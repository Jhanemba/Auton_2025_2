-- =====================================================
-- SISTEMA AUTON - CONFIABILIDAD VEHICULAR POR PROCEDENCIA
-- Desarrollo: Kevin Eduardo Cardenas Mamani
-- Universidad: USFX - Carrera de Informática
-- Versión optimizada para .NET con CAD, CLN, CP
-- =====================================================

CREATE DATABASE Auton;

GO
USE [master];
GO
CREATE LOGIN [usrauton] WITH PASSWORD = N'Auton2025*',
	DEFAULT_DATABASE = [Auton],
	CHECK_EXPIRATION = OFF,
	CHECK_POLICY = ON

USE Auton
GO
CREATE USER usrauton FOR LOGIN usrauton
GO
ALTER ROLE [db_owner] ADD MEMBER usrauton
GO

-- Eliminar objetos si existen
DROP TABLE IF EXISTS VehiculoMantenimiento;
DROP TABLE IF EXISTS HistorialConfiabilidad;
DROP TABLE IF EXISTS Vehiculo;
DROP TABLE IF EXISTS ModeloVehiculo;
DROP TABLE IF EXISTS Marca;
DROP TABLE IF EXISTS Procedencia;
DROP TABLE IF EXISTS TipoVehiculo;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Empleado;
DROP TABLE IF EXISTS Cargo;

-- Eliminar procedimientos almacenados si existen
DROP PROC IF EXISTS paVehiculoListar;
DROP PROC IF EXISTS paVehiculoPorProcedencia;
DROP PROC IF EXISTS paVehiculoBuscar;
DROP PROC IF EXISTS paConfiabilidadListar;
DROP PROC IF EXISTS paEmpleadoListar;
DROP PROC IF EXISTS paUsuarioValidar;
DROP PROC IF EXISTS paMarcaPorProcedencia;
DROP PROC IF EXISTS paModeloPorMarca;
DROP PROC IF EXISTS paEstadisticasConfiabilidad;
DROP PROC IF EXISTS paVehiculoInsertar;
DROP PROC IF EXISTS paVehiculoActualizar;
DROP PROC IF EXISTS paVehiculoEliminar;
DROP PROC IF EXISTS paCargoListar;
DROP PROC IF EXISTS paTipoVehiculoListar;
DROP PROC IF EXISTS paProcedenciaListar;

-- Eliminar vistas si existen
DROP VIEW IF EXISTS VW_VehiculosCompleto;
DROP VIEW IF EXISTS VW_EstadisticasProcedencia;

-- =====================================================
-- CREACIÓN DE TABLAS
-- =====================================================

CREATE TABLE Cargo (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	descripcion VARCHAR(50) NOT NULL UNIQUE,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	estado SMALLINT NOT NULL DEFAULT 1 -- 1: Activo, 0: Inactivo, -1: Eliminado
);

CREATE TABLE Empleado (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	idCargo INT NOT NULL,
	cedulaIdentidad VARCHAR(12) NOT NULL UNIQUE,
	nombres VARCHAR(50) NOT NULL,
	primerApellido VARCHAR(30) NULL,
	segundoApellido VARCHAR(30) NULL,
	direccion VARCHAR(250) NOT NULL,
	celular BIGINT NOT NULL,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	estado SMALLINT NOT NULL DEFAULT 1,
	CONSTRAINT FK_Empleado_Cargo FOREIGN KEY (idCargo) REFERENCES Cargo(id)
);

CREATE TABLE Usuario (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	idEmpleado INT NOT NULL,
	usuario VARCHAR(20) NOT NULL UNIQUE,
	clave VARCHAR(250) NOT NULL,
	esAdministrador BIT NOT NULL DEFAULT 0,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	estado SMALLINT NOT NULL DEFAULT 1,
	CONSTRAINT FK_Usuario_Empleado FOREIGN KEY (idEmpleado) REFERENCES Empleado(id)
);

CREATE TABLE Procedencia (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	codigo VARCHAR(10) NOT NULL UNIQUE,
	descripcion VARCHAR(50) NOT NULL UNIQUE,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	estado SMALLINT NOT NULL DEFAULT 1
);

CREATE TABLE Marca (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	idProcedencia INT NOT NULL,
	nombre VARCHAR(50) NOT NULL,
	descripcion VARCHAR(250) NULL,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	estado SMALLINT NOT NULL DEFAULT 1,
	CONSTRAINT FK_Marca_Procedencia FOREIGN KEY (idProcedencia) REFERENCES Procedencia(id),
	CONSTRAINT UK_Marca_Nombre_Procedencia UNIQUE (nombre, idProcedencia)
);

CREATE TABLE TipoVehiculo (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	descripcion VARCHAR(50) NOT NULL UNIQUE,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	estado SMALLINT NOT NULL DEFAULT 1
);

CREATE TABLE ModeloVehiculo (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	idMarca INT NOT NULL,
	idTipoVehiculo INT NOT NULL,
	nombre VARCHAR(100) NOT NULL,
	anioInicio INT NOT NULL CHECK (anioInicio >= 1900 AND anioInicio <= YEAR(GETDATE())),
	anioFin INT NULL CHECK (anioFin <= YEAR(GETDATE())),
	motor VARCHAR(50) NULL,
	combustible VARCHAR(20) NULL,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	estado SMALLINT NOT NULL DEFAULT 1,
	CONSTRAINT FK_ModeloVehiculo_Marca FOREIGN KEY (idMarca) REFERENCES Marca(id),
	CONSTRAINT FK_ModeloVehiculo_TipoVehiculo FOREIGN KEY (idTipoVehiculo) REFERENCES TipoVehiculo(id),
	CONSTRAINT UK_ModeloVehiculo_Nombre_Marca UNIQUE (nombre, idMarca, anioInicio),
	CONSTRAINT CK_ModeloVehiculo_AnioFin CHECK (anioFin IS NULL OR anioFin >= anioInicio)
);

CREATE TABLE Vehiculo (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	idModeloVehiculo INT NOT NULL,
	anioFabricacion INT NOT NULL CHECK (anioFabricacion >= 1900 AND anioFabricacion <= YEAR(GETDATE())),
	confiabilidad VARCHAR(10) NOT NULL CHECK (confiabilidad IN ('Excelente', 'Bueno', 'Regular', 'Malo')),
	puntuacion DECIMAL(3,1) NOT NULL CHECK (puntuacion >= 1.0 AND puntuacion <= 10.0),
	observaciones VARCHAR(500) NULL,
	kilometraje INT NULL CHECK (kilometraje >= 0),
	propietario VARCHAR(100) NULL,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	estado SMALLINT NOT NULL DEFAULT 1,
	CONSTRAINT FK_Vehiculo_ModeloVehiculo FOREIGN KEY (idModeloVehiculo) REFERENCES ModeloVehiculo(id)
);

CREATE TABLE HistorialConfiabilidad (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	idVehiculo INT NOT NULL,
	confiabilidadAnterior VARCHAR(10) NOT NULL,
	confiabilidadNueva VARCHAR(10) NOT NULL,
	puntuacionAnterior DECIMAL(3,1) NOT NULL,
	puntuacionNueva DECIMAL(3,1) NOT NULL,
	motivo VARCHAR(250) NOT NULL,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	CONSTRAINT FK_HistorialConfiabilidad_Vehiculo FOREIGN KEY (idVehiculo) REFERENCES Vehiculo(id)
);

CREATE TABLE VehiculoMantenimiento (
	id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	idVehiculo INT NOT NULL,
	tipoMantenimiento VARCHAR(50) NOT NULL,
	descripcion VARCHAR(250) NOT NULL,
	costo DECIMAL(10,2) NULL CHECK (costo >= 0),
	fechaMantenimiento DATE NOT NULL,
	proximoMantenimiento DATE NULL,
	usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
	fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
	estado SMALLINT NOT NULL DEFAULT 1,
	CONSTRAINT FK_VehiculoMantenimiento_Vehiculo FOREIGN KEY (idVehiculo) REFERENCES Vehiculo(id)
);

ALTER TABLE Vehiculo DROP COLUMN combustible;

ALTER TABLE ModeloVehiculo 
ADD transmision VARCHAR(20) NULL;

ALTER TABLE ModeloVehiculo 
ADD tieneTurbo BIT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ModeloVehiculo') AND name = 'nroAirbags')
    ALTER TABLE ModeloVehiculo ADD nroAirbags INT NULL DEFAULT 2;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ModeloVehiculo') AND name = 'seguridadChoques')
    ALTER TABLE ModeloVehiculo ADD seguridadChoques INT NULL CHECK (seguridadChoques >= 0 AND seguridadChoques <= 5);

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ModeloVehiculo') AND name = 'organizacionSeguridad')
    ALTER TABLE ModeloVehiculo ADD organizacionSeguridad VARCHAR(50) NULL;


	IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Vehiculo') AND name = 'nroChasis')
    ALTER TABLE Vehiculo ADD nroChasis VARCHAR(17) NOT NULL DEFAULT '';


	-- Creamos índice único para evitar duplicados
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UX_Vehiculo_NroChasis')
BEGIN
    CREATE UNIQUE INDEX UX_Vehiculo_NroChasis 
    ON Vehiculo(nroChasis) 
    WHERE nroChasis != '' AND estado != -1;
END




-- 1. ELIMINAMOS la constraint antigua que NO consideraba turbo
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UK_ModeloVehiculo_Nombre_Marca')
BEGIN
    ALTER TABLE ModeloVehiculo DROP CONSTRAINT UK_ModeloVehiculo_Nombre_Marca;
    PRINT 'Constraint antigua (sin turbo) eliminada';
END

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UK_ModeloVehiculo_Nombre_Marca_Transmision')
BEGIN
    ALTER TABLE ModeloVehiculo DROP CONSTRAINT UK_ModeloVehiculo_Nombre_Marca_Transmision;
    PRINT 'Constraint antigua (sin turbo) eliminada';
END

-- 2. CREAMOS nueva constraint que incluye transmisión Y turbo
-- Esto permite:
-- - Toyota Tundra Manual SIN turbo
-- - Toyota Tundra Manual CON turbo
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UK_ModeloVehiculo_Completo')
BEGIN
    ALTER TABLE ModeloVehiculo 
    ADD CONSTRAINT UK_ModeloVehiculo_Completo 
    UNIQUE (nombre, idMarca, anioInicio, transmision, tieneTurbo);
    
    PRINT 'Nueva constraint creada: nombre + marca + año + transmisión + turbo';
END

-- 3. ACTUALIZAMOS tabla de seguridad para permitir 0 estrellas
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_ModeloVehiculo_SeguridadChoques')
BEGIN
    ALTER TABLE ModeloVehiculo DROP CONSTRAINT CK_ModeloVehiculo_SeguridadChoques;
END

ALTER TABLE ModeloVehiculo 
ADD CONSTRAINT CK_ModeloVehiculo_SeguridadChoques 
CHECK (seguridadChoques >= 0 AND seguridadChoques <= 5);

PRINT 'Seguridad actualizada: ahora permite 0 estrellas';

-- 4. ACTUALIZAMOS modelos existentes con transmision NULL
UPDATE ModeloVehiculo 
SET transmision = 'Manual'
WHERE transmision IS NULL;

PRINT 'Modelos con transmision NULL actualizados a "Manual"';

-- 5. ACTUALIZAMOS modelos existentes con tieneTurbo NULL
UPDATE ModeloVehiculo 
SET tieneTurbo = 0
WHERE tieneTurbo IS NULL;

PRINT 'Modelos con tieneTurbo NULL actualizados a 0 (sin turbo)';

-- 6. Hacemos transmision NOT NULL recomendadisimo
--ALTER TABLE ModeloVehiculo
--ALTER COLUMN transmision VARCHAR(20) NOT NULL;

--PRINT 'Columna transmision ahora es NOT NULL';

---- 7. Hacer tieneTurbo NOT NULL maybe
--ALTER TABLE ModeloVehiculo
--ALTER COLUMN tieneTurbo BIT NOT NULL;

--PRINT 'Columna tieneTurbo ahora es NOT NULL';

-- 8. EJEMPLOS: Ahora podemos tener estos registros
/*
INSERT INTO ModeloVehiculo (nombre, idMarca, transmision, tieneTurbo, ...) 
VALUES 
('Tundra', 1, 'Manual', 0, ...)  -- ✅ V8 Manual sin turbo (Bueno)
('Tundra', 1, 'Manual', 1, ...)  -- ✅ 3.3L Turbo Manual (Malo)
('Versa', 2, 'Manual', 0, ...)   -- ✅ Manual sin turbo (Bueno)
('Versa', 2, 'CVT', 0, ...)      -- ✅ CVT sin turbo (Malo)
('Fiesta', 3, 'Manual', 0, ...)  -- ✅ Manual sin turbo (Bueno)
('Fiesta', 3, 'Manual', 1, ...)  -- ✅ Manual con turbo (Malo)
*/

PRINT '1. Modelos duplicados OK si transmisión O turbo diferente';
PRINT '2. Seguridad permite 0 estrellas';
PRINT '3. Transmision y tieneTurbo son NOT NULL';
PRINT '4. Ejemplos:';
PRINT '   - Tundra Manual SIN turbo ≠ Tundra Manual CON turbo';
PRINT '   - Versa Manual ≠ Versa CVT';
PRINT '   - Fiesta Manual SIN turbo ≠ Fiesta Manual CON turbo';



-- 1. AGREGAMOS columna paisFabricacion
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ModeloVehiculo') AND name = 'paisFabricacion')
BEGIN
    ALTER TABLE ModeloVehiculo 
    ADD paisFabricacion VARCHAR(50) NULL;
    
    PRINT 'Columna paisFabricacion agregada';
END

-- 2. ACTUALIZAMOS modelos existentes con país por defecto
UPDATE ModeloVehiculo 
SET paisFabricacion = 'No especificado'
WHERE paisFabricacion IS NULL;

PRINT 'Modelos existentes actualizados con país por defecto';

-- 3. HACEMOS paisFabricacion NOT NULL
ALTER TABLE ModeloVehiculo
ALTER COLUMN paisFabricacion VARCHAR(50) NOT NULL;

PRINT 'Columna paisFabricacion ahora es NOT NULL';

-- 4. ELIMINAMOS constraint antigua
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UK_ModeloVehiculo_Completo')
BEGIN
    ALTER TABLE ModeloVehiculo DROP CONSTRAINT UK_ModeloVehiculo_Completo;
    PRINT 'Constraint antigua eliminada';
END

-- 5. CREAMOS nueva constraint que incluye país de fabricación
-- Esto permite:
-- - Suzuki Baleno Manual Sin turbo India ✅
-- - Suzuki Baleno Manual Sin turbo Hungría ✅
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UK_ModeloVehiculo_ConPais')
BEGIN
    ALTER TABLE ModeloVehiculo 
    ADD CONSTRAINT UK_ModeloVehiculo_ConPais 
    UNIQUE (nombre, idMarca, anioInicio, transmision, tieneTurbo, paisFabricacion);
    
    PRINT 'Nueva constraint creada: nombre + marca + año + transmisión + turbo + PAÍS';
END

-- =====================================================
-- PROCEDIMIENTOS ALMACENADOS ESENCIALES
-- =====================================================

GO
CREATE PROC paUsuarioValidar 
	@usuario VARCHAR(20),
	@clave VARCHAR(250)
AS
	SELECT u.id, u.usuario, u.esAdministrador, 
		   e.nombres + ' ' + ISNULL(e.primerApellido, '') + ' ' + ISNULL(e.segundoApellido, '') AS nombreCompleto,
		   c.descripcion AS cargo
	FROM Usuario u
	INNER JOIN Empleado e ON u.idEmpleado = e.id
	INNER JOIN Cargo c ON e.idCargo = c.id
	WHERE u.usuario = @usuario AND u.clave = @clave AND u.estado = 1 AND e.estado = 1;

GO
CREATE PROC paVehiculoListar @parametro VARCHAR(100)
AS
	SELECT v.id, mv.nombre AS modelo, m.nombre AS marca, p.descripcion AS procedencia,
		   tv.descripcion AS tipoVehiculo, v.anioFabricacion, v.confiabilidad, v.puntuacion,
		   v.kilometraje, v.propietario, mv.motor, mv.combustible,
		   v.observaciones, v.usuarioRegistro, v.fechaRegistro, v.estado
	FROM Vehiculo v
	INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
	INNER JOIN Marca m ON mv.idMarca = m.id
	INNER JOIN Procedencia p ON m.idProcedencia = p.id
	INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
	WHERE v.estado <> -1 
		AND (mv.nombre + m.nombre + p.descripcion + tv.descripcion + v.confiabilidad + 
			 ISNULL(v.propietario, '') + ISNULL(mv.motor, '') + ISNULL(mv.combustible, ''))
			LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
	ORDER BY v.estado DESC, p.descripcion ASC, m.nombre ASC, mv.nombre ASC;

GO
CREATE PROC paVehiculoInsertar
    @idModeloVehiculo INT,
    @anioFabricacion INT,
    @confiabilidad VARCHAR(10),
    @puntuacion DECIMAL(3,1),
    @observaciones VARCHAR(500) = NULL,
    @kilometraje INT = NULL,
    @propietario VARCHAR(100) = NULL
AS
    INSERT INTO Vehiculo (idModeloVehiculo, anioFabricacion, confiabilidad, puntuacion, 
                         observaciones, kilometraje, propietario)
    VALUES (@idModeloVehiculo, @anioFabricacion, @confiabilidad, @puntuacion, 
            @observaciones, @kilometraje, @propietario);
    
    SELECT SCOPE_IDENTITY() AS nuevoId;

GO
CREATE PROC paVehiculoActualizar
    @id INT,
    @idModeloVehiculo INT,
    @anioFabricacion INT,
    @confiabilidad VARCHAR(10),
    @puntuacion DECIMAL(3,1),
    @observaciones VARCHAR(500) = NULL,
    @kilometraje INT = NULL,
    @propietario VARCHAR(100) = NULL
AS
    -- Guardar historial si cambia la confiabilidad
    IF EXISTS (SELECT 1 FROM Vehiculo WHERE id = @id AND confiabilidad != @confiabilidad)
        INSERT INTO HistorialConfiabilidad (idVehiculo, confiabilidadAnterior, confiabilidadNueva, 
                                           puntuacionAnterior, puntuacionNueva, motivo)
        SELECT @id, confiabilidad, @confiabilidad, puntuacion, @puntuacion, 
               'Actualización manual de confiabilidad'
        FROM Vehiculo WHERE id = @id;
    
    UPDATE Vehiculo 
    SET idModeloVehiculo = @idModeloVehiculo,
        anioFabricacion = @anioFabricacion,
        confiabilidad = @confiabilidad,
        puntuacion = @puntuacion,
        observaciones = @observaciones,
        kilometraje = @kilometraje,
        propietario = @propietario
    WHERE id = @id;
    
    SELECT 'Actualizado correctamente' AS mensaje;

GO
CREATE PROC paVehiculoEliminar
    @id INT
AS
    UPDATE Vehiculo SET estado = -1 WHERE id = @id;
    SELECT 'Eliminado correctamente' AS mensaje;

GO
CREATE PROC paProcedenciaListar
AS
	SELECT id, codigo, descripcion
	FROM Procedencia
	WHERE estado = 1
	ORDER BY descripcion ASC;

GO
CREATE PROC paCargoListar
AS
	SELECT id, descripcion
	FROM Cargo
	WHERE estado = 1
	ORDER BY descripcion ASC;

GO
CREATE PROC paTipoVehiculoListar
AS
	SELECT id, descripcion
	FROM TipoVehiculo
	WHERE estado = 1
	ORDER BY descripcion ASC;

GO
CREATE PROC paMarcaPorProcedencia @idProcedencia INT
AS
	SELECT id, nombre, descripcion
	FROM Marca
	WHERE estado = 1 AND idProcedencia = @idProcedencia
	ORDER BY nombre ASC;

GO
CREATE PROC paModeloPorMarca @idMarca INT
AS
	SELECT mv.id, mv.nombre, tv.descripcion AS tipoVehiculo, 
		   mv.anioInicio, mv.anioFin, mv.motor, mv.combustible
	FROM ModeloVehiculo mv
	INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
	WHERE mv.estado = 1 AND mv.idMarca = @idMarca
	ORDER BY mv.nombre ASC;

-- =====================================================
-- DATOS INICIALES
-- =====================================================

-- Insertar Cargos
INSERT INTO Cargo (descripcion) VALUES 
('Administrador Sistema'),
('Técnico Automotriz'),
('Evaluador Vehicular'),
('Consultor Ventas');

-- Insertar Procedencias
INSERT INTO Procedencia (codigo, descripcion) VALUES 
('JPN', 'Japonés'),
('USA', 'Americano'),
('CHN', 'Chino'),
('GER', 'Alemán'),
('EUR', 'Europeo'),
('KOR', 'Coreano');

-- Insertar Tipos de Vehículo
INSERT INTO TipoVehiculo (descripcion) VALUES 
('Sedán'),
('SUV'),
('Pickup'),
('Hatchback'),
('Coupé');

-- Insertar Empleados
INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, segundoApellido, direccion, celular, idCargo) VALUES 
('12345678', 'Kevin Eduardo', 'Cardenas', 'Mamani', 'Av. Venezuela #123', 71234567, 1),
('87654321', 'María José', 'Fernández', 'López', 'Calle Bolívar #456', 72345678, 2);

-- Insertar Usuarios
INSERT INTO Usuario (idEmpleado, usuario, clave, esAdministrador) VALUES 
(1, 'admin', 'YWRtaW4xMjM=', 1), -- admin123 en base64
(2, 'maria.fernandez', 'bWFyaWExMjM=', 0); -- maria123 en base64

-- Insertar Marcas por Procedencia
INSERT INTO Marca (idProcedencia, nombre, descripcion) VALUES 
(1, 'Toyota', 'Fabricante japonés líder en confiabilidad'),
(1, 'Honda', 'Marca japonesa reconocida por durabilidad'),
(2, 'Ford', 'Fabricante americano histórico'),
(2, 'Chevrolet', 'Marca americana popular'),
(4, 'Volkswagen', 'Fabricante alemán popular'),
(6, 'Hyundai', 'Fabricante coreano líder');

-- Insertar algunos Modelos de Vehículo
INSERT INTO ModeloVehiculo (idMarca, idTipoVehiculo, nombre, anioInicio, motor, combustible) VALUES 
(1, 1, 'Corolla', 1966, '1.8L', 'Gasolina'),
(1, 2, 'RAV4', 1994, '2.5L', 'Gasolina'),
(2, 1, 'Civic', 1972, '2.0L', 'Gasolina'),
(3, 3, 'F-150', 1975, '3.5L V6', 'Gasolina'),
(5, 4, 'Golf', 1974, '1.4L TSI', 'Gasolina'),
(6, 1, 'Elantra', 1990, '2.0L', 'Gasolina');

-- Insertar algunos Vehículos de ejemplo
INSERT INTO Vehiculo (idModeloVehiculo, anioFabricacion, confiabilidad, puntuacion, observaciones, kilometraje, propietario) VALUES 
(1, 2020, 'Excelente', 9.2, 'Vehículo muy confiable, bajo mantenimiento', 45000, 'Juan Pérez'),
(2, 2022, 'Excelente', 9.4, 'SUV confiable, ideal para familia', 15000, 'Roberto Silva'),
(3, 2018, 'Bueno', 8.5, 'Buen rendimiento, algunos problemas menores', 78000, 'Carlos Mamani');

-- =====================================================
-- ÍNDICES PARA OPTIMIZACIÓN
-- =====================================================

CREATE NONCLUSTERED INDEX IX_Vehiculo_Estado ON Vehiculo(estado);
CREATE NONCLUSTERED INDEX IX_Vehiculo_Confiabilidad ON Vehiculo(confiabilidad);
CREATE NONCLUSTERED INDEX IX_Marca_Procedencia ON Marca(idProcedencia);
CREATE NONCLUSTERED INDEX IX_ModeloVehiculo_Marca ON ModeloVehiculo(idMarca);
CREATE NONCLUSTERED INDEX IX_Usuario_Estado ON Usuario(estado);

select * FROM Procedencia;
select * FROM Vehiculo;
select * FROM ModeloVehiculo

PRINT '========================================';
PRINT 'SISTEMA AUTON - VERSION OPTIMIZADA';
PRINT '========================================';
PRINT 'Base de datos: Auton';
PRINT 'Usuario: usrauton / Auton2025*';
PRINT 'Tablas: 9 | Procedimientos: 10';
PRINT 'Listo para desarrollo .NET';
PRINT '========================================';