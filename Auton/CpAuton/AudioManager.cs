using AxWMPLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpAuton
{
    public static class AudioManager
    {
        private static AxWindowsMediaPlayer _player;
        private static List<string> _playlist = new List<string>();
        private static List<int> _volumes = new List<int>();
        private static int _index = 0;
        private static bool _userStopped = false;
        private static int _lastPlayState = 0;
        // --- NUEVO: Propiedad para controlar el bucle desde cualquier formulario ---
        public static bool LoopPlaylist { get; set; } = true;

        public static void Inicializar(AxWindowsMediaPlayer reproductor)
        {
            _player = reproductor;

            if (_player != null)
            {
                _player.PlayStateChange += Player_PlayStateChange;
                _lastPlayState = 0;
            }
        }

        // ----------- NUEVO: Playlist -------------
        public static void SetPlaylist(List<string> canciones)
        {
            _playlist = canciones ?? new List<string>();
            _volumes = new List<int>(); // resetear volúmenes por pista
            _index = 0;
            _userStopped = false;
        }

        // Compatibilidad: llamada original, volumen uniforme para todas las pistas
        public static void PlayPlaylist(int volumen = 60)
        {
            if (_player == null || _playlist.Count == 0) return;

            _userStopped = false;
            _volumes.Clear(); // indicar que usamos volumen uniforme
            _player.settings.volume = volumen;
            _player.settings.setMode("loop", false);
            try
            {
                _player.settings.autoStart = true;
            }
            catch { }

            // establecer la primera URL y reproducir
            _player.URL = _playlist[_index];
            try { _player.Ctlcontrols.play(); } catch { }
        }

        // Nueva sobrecarga: pasar volúmenes por pista. Ej: PlayPlaylist(60,15)
        public static void PlayPlaylist(params int[] volumenes)
        {
            if (_player == null || _playlist.Count == 0) return;

            _userStopped = false;

            // Construir lista de volúmenes acorde al número de pistas.
            // Si se pasan menos valores que pistas, se repite el último valor para las restantes.
            _volumes = new List<int>();
            if (volumenes == null || volumenes.Length == 0)
            {
                // sin volúmenes específicos -> usar por defecto 60 (se indicará en la reproducción)
            }
            else
            {
                for (int i = 0; i < _playlist.Count; i++)
                {
                    int v;
                    if (i < volumenes.Length)
                        v = volumenes[i];
                    else
                        v = volumenes[volumenes.Length - 1]; // repetir último
                    if (v < 0) v = 0;
                    if (v > 100) v = 100;
                    _volumes.Add(v);
                }
            }

            int inicialVol = (_volumes != null && _volumes.Count > 0) ? _volumes[0] : 60;
            _player.settings.volume = inicialVol;
            _player.settings.setMode("loop", false);
            try
            {
                _player.settings.autoStart = true;
            }
            catch { }

            // establecer la primera URL y reproducir
            _player.URL = _playlist[_index];
            try { _player.Ctlcontrols.play(); } catch { }
        }

        // Cuando termina una canción
        private static void Player_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (_player == null) return;

            // actualizar estado anterior / actual
            int prevState = _lastPlayState;
            _lastPlayState = e.newState;

            if (_userStopped) return; // si el usuario paró manualmente, no avanzar

            bool mediaEnded = false;

            // 8 = MediaEnded -> fin claro
            if (e.newState == 8)
            {
                mediaEnded = true;
            }
            else
            {
                // Algunos sistemas/reportes muestran 1 (Stopped) o 10 (Ready) al terminar.
                // Detectar transición desde Playing (3) o MediaEnded (8) a Stopped/Ready y confirmar posición final.
                if ((e.newState == 1 || e.newState == 10) && (prevState == 3 || prevState == 8))
                {
                    try
                    {
                        var media = _player.currentMedia;
                        if (media != null)
                        {
                            double dur = media.duration;
                            double pos = _player.Ctlcontrols.currentPosition;
                            // Si la duración es válida y la posición está muy cerca del final, asumimos fin de media
                            if (dur > 0 && (dur - pos) <= 1.5)
                            {
                                mediaEnded = true;
                            }
                            else
                            {
                                // En algunos casos duration puede ser 0 inicialmente; como respaldo
                                // si prevState ya era MediaEnded, avanzar también
                                if (prevState == 8)
                                    mediaEnded = true;
                            }
                        }
                        else
                        {
                            // si no hay currentMedia, asumir fin
                            mediaEnded = true;
                        }
                    }
                    catch
                    {
                        // en caso de error al consultar, asumir fin para no bloquear playlist
                        mediaEnded = true;
                    }
                }
            }

            if (mediaEnded)
            {
                if (_playlist.Count <= 1) return; // si es 1 canción, no hace ciclo

                // avanzar al siguiente índice
                _index++;
                if (_index >= _playlist.Count)
                {
                    _index = 0; // vuelve a la primera canción

                    // --- NUEVO: Si LoopPlaylist es false, detiene la reproducción y sale ---
                    if (!LoopPlaylist)
                    {
                        try
                        {
                            if (_player != null && _player.IsHandleCreated)
                            {
                                _player.BeginInvoke(new Action(() => _player.Ctlcontrols.stop()));
                            }
                            else
                            {
                                _player?.Ctlcontrols.stop();
                            }
                        }
                        catch { }
                        return; // Detiene la música y no pasa a la primera canción
                    }
                }

                try
                {
                    // Asegurarse de ejecutar en el hilo del control para que play funcione correctamente
                    if (_player != null && _player.IsHandleCreated)
                    {
                        _player.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                _player.settings.autoStart = true;
                                _player.Ctlcontrols.stop();

                                // aplicar volumen por pista si existe
                                if (_volumes != null && _volumes.Count > _index)
                                {
                                    _player.settings.volume = _volumes[_index];
                                }

                                _player.URL = _playlist[_index];
                                _player.Ctlcontrols.play();
                            }
                            catch
                            {
                                // ignorar errores
                            }
                        }));
                    }
                    else
                    {
                        // fallback sin BeginInvoke
                        _player.settings.autoStart = true;
                        _player.Ctlcontrols.stop();

                        if (_volumes != null && _volumes.Count > _index)
                        {
                            _player.settings.volume = _volumes[_index];
                        }

                        _player.URL = _playlist[_index];
                        _player.Ctlcontrols.play();
                    }
                }
                catch
                {
                    // evitar excepciones en el hilo del UI
                }
            }
        }

        // ----------- Forma original (1 canción) ------------

        public static void Reproducir(string ruta, bool loop = true, int volumen = 60)
        {
            if (_player == null) return;

            _playlist.Clear(); // no usar playlist
            _volumes.Clear();
            _userStopped = false;
            _player.settings.volume = volumen;
            _player.settings.setMode("loop", loop);
            try { _player.settings.autoStart = true; } catch { }
            _player.URL = ruta;
            _player.Ctlcontrols.play();
        }

        public static void Detener()
        {
            if (_player == null) return;
            _userStopped = true;
            _player.Ctlcontrols.stop();
        }
    }
}
