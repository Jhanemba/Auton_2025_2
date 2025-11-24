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
CREATE ROLE [db_owner] ADD MEMBER usrauton
GO

USE Auton;


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

DROP PROC IF EXISTS paVehiculosJaponeses;
DROP PROC IF EXISTS paVehiculosAmericanos;
DROP PROC IF EXISTS paVehiculosChinos;
DROP PROC IF EXISTS paVehiculosAlemanes;
DROP PROC IF EXISTS paVehiculosEuropeos;
DROP PROC IF EXISTS paVehiculosCoreanos;

-- Eliminar vistas si existen
DROP VIEW IF EXISTS VW_VehiculosCompleto;
DROP VIEW IF EXISTS VW_EstadisticasProcedencia;




PRINT '========================================';
PRINT 'CREANDO SISTEMA AUTON SIMPLIFICADO';
PRINT '========================================';

-- =====================================================
-- CREACIÓN DE TABLAS
-- =====================================================

CREATE TABLE Cargo (
    id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    descripcion VARCHAR(50) NOT NULL UNIQUE,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1
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

-- MODELO SIMPLIFICADO: Solo nombre + marca + tipo
CREATE TABLE ModeloVehiculo (
    id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    idMarca INT NOT NULL,
    idTipoVehiculo INT NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(500) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT FK_ModeloVehiculo_Marca FOREIGN KEY (idMarca) REFERENCES Marca(id),
    CONSTRAINT FK_ModeloVehiculo_TipoVehiculo FOREIGN KEY (idTipoVehiculo) REFERENCES TipoVehiculo(id),
    CONSTRAINT UK_ModeloVehiculo_Simple UNIQUE (nombre, idMarca)
);

-- VEHICULO COMPLETO: TODAS las características específicas
CREATE TABLE Vehiculo (
    id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    nroChasis VARCHAR(17) NOT NULL,
    idModeloVehiculo INT NOT NULL,
    
    -- Características específicas
    generacion VARCHAR(50) NOT NULL,
    paisFabricacion VARCHAR(50) NOT NULL,
    transmision VARCHAR(20) NOT NULL,
    tieneTurbo BIT NOT NULL DEFAULT 0,
    motor VARCHAR(50) NOT NULL,
    combustible VARCHAR(20) NOT NULL,
    
    -- Seguridad
    nroAirbags INT NOT NULL DEFAULT 2,
    seguridadChoques INT NOT NULL DEFAULT 0 CHECK (seguridadChoques >= 0 AND seguridadChoques <= 5),
    organizacionSeguridad VARCHAR(50) NOT NULL DEFAULT 'Sin calificación',
    
    -- Evaluación
    anioFabricacion INT NOT NULL CHECK (anioFabricacion >= 1900 AND anioFabricacion <= YEAR(GETDATE())),
    confiabilidad VARCHAR(10) NOT NULL CHECK (confiabilidad IN ('Excelente', 'Bueno', 'Regular', 'Malo')),
    puntuacion DECIMAL(3,1) NOT NULL CHECK (puntuacion >= 1.0 AND puntuacion <= 10.0),
    kilometraje INT NULL CHECK (kilometraje >= 0),
    observaciones VARCHAR(500) NULL,
    
    -- Metadata
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    
    CONSTRAINT FK_Vehiculo_ModeloVehiculo FOREIGN KEY (idModeloVehiculo) REFERENCES ModeloVehiculo(id)
);

-- Índice único para nroChasis
CREATE UNIQUE INDEX UX_Vehiculo_NroChasis 
ON Vehiculo(nroChasis) 
WHERE nroChasis != '' AND estado != -1;

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


CREATE TABLE Vehiculo
ADD fotografia VARBINARY(MAX) NULL;

PRINT 'Campo fotografia agregado a la tabla Vehiculo';

ALTER TABLE Vehiculo
ADD nroVelocidades INT NULL 
    CHECK (nroVelocidades >= 4 AND nroVelocidades <= 10);

PRINT 'Campo nroVelocidades agregado a la tabla Vehiculo';


GO
ALTER TABLE Vehiculo
ADD nroAsientos INT NULL 
    CHECK (nroAsientos >= 3 AND nroAsientos <= 8);

GO
ALTER TABLE Vehiculo
ADD nroPuertas INT NOT NULL DEFAULT 5
    CHECK (nroPuertas IN (3, 4, 5));

-- =====================================================
-- PROCEDIMIENTOS ALMACENADOS
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
ALTER PROC paVehiculoListar @parametro VARCHAR(100)
AS
    SELECT v.id, 
           v.nroChasis,
           mv.nombre AS modelo, 
           m.nombre AS marca, 
           p.descripcion AS procedencia,
           tv.descripcion AS tipoVehiculo, 
           v.generacion, 
           v.paisFabricacion, 
           v.transmision, 
           v.nroVelocidades,
           v.nroAsientos,
           v.nroPuertas,
           v.tieneTurbo,
           v.motor, 
           v.combustible, 
           v.nroAirbags, 
           v.seguridadChoques, 
           v.organizacionSeguridad,
           v.anioFabricacion, 
           v.confiabilidad, 
           v.puntuacion,
           v.kilometraje, 
           v.observaciones, 
           v.usuarioRegistro, 
           v.fechaRegistro, 
           v.estado
    FROM Vehiculo v
    INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
    INNER JOIN Marca m ON mv.idMarca = m.id
    INNER JOIN Procedencia p ON m.idProcedencia = p.id
    INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
    WHERE v.estado <> -1 
        AND (
            CONCAT(
                ISNULL(mv.nombre,''), ' ',
                ISNULL(m.nombre,''), ' ',
                ISNULL(p.descripcion,''), ' ',
                ISNULL(tv.descripcion,''), ' ',
                ISNULL(v.confiabilidad,''), ' ',
                ISNULL(v.generacion,''), ' ',
                ISNULL(v.paisFabricacion,''), ' ',
                CAST(ISNULL(v.anioFabricacion, 0) AS VARCHAR(10)), ' ',
                ISNULL(v.transmision,''), ' ',
                ISNULL(v.motor,''), ' ',
                CAST(ISNULL(v.puntuacion, 0) AS VARCHAR(10)), ' ',
                ISNULL(v.combustible,'')
            )
        ) LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
    ORDER BY v.estado DESC, p.descripcion ASC, m.nombre ASC, mv.nombre ASC;


--GO
--ALTER PROC paVehiculoListar @parametro VARCHAR(100)
--AS
--    SELECT v.id, 
--           v.nroChasis,  -- ⭐ AGREGAR ESTA LÍNEA
--           mv.nombre AS modelo, 
--           m.nombre AS marca, 
--           p.descripcion AS procedencia,
--           tv.descripcion AS tipoVehiculo, 
--           v.generacion, v.paisFabricacion, v.transmision, v.tieneTurbo,
--           v.motor, v.combustible, v.nroAirbags, v.seguridadChoques, v.organizacionSeguridad,
--           v.anioFabricacion, v.confiabilidad, v.puntuacion,
--           v.kilometraje, v.observaciones, 
--           v.usuarioRegistro, v.fechaRegistro, v.estado
--    FROM Vehiculo v
--    INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
--    INNER JOIN Marca m ON mv.idMarca = m.id
--    INNER JOIN Procedencia p ON m.idProcedencia = p.id
--    INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
--    WHERE v.estado <> -1 
--        AND (mv.nombre + m.nombre + p.descripcion + tv.descripcion + v.confiabilidad + 
--             v.generacion + v.paisFabricacion + v.anioFabricacion + v.transmision + ISNULL(v.motor, '') + v.puntuacion + v.combustible)
--            LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
--    ORDER BY v.estado DESC, p.descripcion ASC, m.nombre ASC, mv.nombre ASC;

GO
ALTER PROC paVehiculoInsertar
    @nroChasis VARCHAR(17),
    @idModeloVehiculo INT,
    @generacion VARCHAR(50),
    @paisFabricacion VARCHAR(50),
    @transmision VARCHAR(20),
    @nroVelocidades INT = NULL,
    @nroPuertas INT = 5,         -- ⭐ NUEVO PARÁMETRO
    @nroAsientos INT,
    @tieneTurbo BIT,
    @motor VARCHAR(50),
    @combustible VARCHAR(20),
    @nroAirbags INT,
    @seguridadChoques INT,
    @organizacionSeguridad VARCHAR(50),
    @anioFabricacion INT,
    @confiabilidad VARCHAR(10),
    @puntuacion DECIMAL(3,1),
    @kilometraje INT = NULL,
    @observaciones VARCHAR(500) = NULL,
    @fotografia VARBINARY(MAX) = NULL
AS
BEGIN
    IF @transmision = 'CVT' AND @nroVelocidades IS NOT NULL
    BEGIN
        RAISERROR('Las transmisiones CVT no tienen velocidades discretas', 16, 1);
        RETURN;
    END

    IF @transmision != 'CVT' AND @nroVelocidades IS NULL
    BEGIN
        RAISERROR('Debe especificar el número de velocidades para este tipo de transmisión', 16, 1);
        RETURN;
    END

    INSERT INTO Vehiculo (
        nroChasis, idModeloVehiculo, generacion, paisFabricacion, transmision, 
        nroVelocidades, nroPuertas, nroAsientos,
        tieneTurbo, motor, combustible, nroAirbags, seguridadChoques, organizacionSeguridad,
        anioFabricacion, confiabilidad, puntuacion, kilometraje, observaciones, fotografia
    )
    VALUES (
        @nroChasis, @idModeloVehiculo, @generacion, @paisFabricacion, @transmision, 
        @nroVelocidades, @nroPuertas, @nroAsientos,
        @tieneTurbo, @motor, @combustible, @nroAirbags, @seguridadChoques, @organizacionSeguridad,
        @anioFabricacion, @confiabilidad, @puntuacion, @kilometraje, @observaciones, @fotografia
    );
    
    SELECT SCOPE_IDENTITY() AS nuevoId;
END


GO
ALTER PROC paVehiculoActualizar
    @id INT,
    @generacion VARCHAR(50),
    @paisFabricacion VARCHAR(50),
    @transmision VARCHAR(20),
    @nroVelocidades INT = NULL,
    @nroPuertas INT,             -- ⭐ NUEVO PARÁMETRO
    @nroAsientos INT,
    @tieneTurbo BIT,
    @motor VARCHAR(50),
    @combustible VARCHAR(20),
    @nroAirbags INT,
    @seguridadChoques INT,
    @organizacionSeguridad VARCHAR(50),
    @anioFabricacion INT,
    @confiabilidad VARCHAR(10),
    @puntuacion DECIMAL(3,1),
    @kilometraje INT = NULL,
    @observaciones VARCHAR(500) = NULL,
    @fotografia VARBINARY(MAX) = NULL
AS
BEGIN
    IF @transmision = 'CVT' AND @nroVelocidades IS NOT NULL
    BEGIN
        RAISERROR('Las transmisiones CVT no tienen velocidades discretas', 16, 1);
        RETURN;
    END

    IF @transmision != 'CVT' AND @nroVelocidades IS NULL
    BEGIN
        RAISERROR('Debe especificar el número de velocidades para este tipo de transmisión', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM Vehiculo WHERE id = @id AND confiabilidad != @confiabilidad)
        INSERT INTO HistorialConfiabilidad (idVehiculo, confiabilidadAnterior, confiabilidadNueva, 
                                           puntuacionAnterior, puntuacionNueva, motivo)
        SELECT @id, confiabilidad, @confiabilidad, puntuacion, @puntuacion, 
               'Actualización manual de confiabilidad'
        FROM Vehiculo WHERE id = @id;
    
    UPDATE Vehiculo 
    SET generacion = @generacion,
        paisFabricacion = @paisFabricacion,
        transmision = @transmision,
        nroVelocidades = @nroVelocidades,
        nroPuertas = @nroPuertas,      -- ⭐ ACTUALIZAR CAMPO
        nroAsientos = @nroAsientos,
        tieneTurbo = @tieneTurbo,
        motor = @motor,
        combustible = @combustible,
        nroAirbags = @nroAirbags,
        seguridadChoques = @seguridadChoques,
        organizacionSeguridad = @organizacionSeguridad,
        anioFabricacion = @anioFabricacion,
        confiabilidad = @confiabilidad,
        puntuacion = @puntuacion,
        kilometraje = @kilometraje,
        observaciones = @observaciones,
        fotografia = ISNULL(@fotografia, fotografia)
    WHERE id = @id;
    
    SELECT 'Actualizado correctamente' AS mensaje;
END



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
    SELECT mv.id, mv.nombre, mv.descripcion, tv.descripcion AS tipoVehiculo
    FROM ModeloVehiculo mv
    INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
    WHERE mv.estado = 1 AND mv.idMarca = @idMarca
    ORDER BY mv.nombre ASC;



-- =====================================================
-- PROCEDIMIENTO: Vehículos Japoneses
-- =====================================================
GO
ALTER PROC paVehiculosJaponeses @parametro VARCHAR(100) = ''
AS
BEGIN
    SELECT 
        v.id, mv.nombre AS modelo, m.nombre AS marca, p.descripcion AS procedencia,
        tv.descripcion AS tipoVehiculo, v.generacion, v.paisFabricacion, 
        v.transmision, v.nroVelocidades, v.nroPuertas, v.nroAsientos, v.tieneTurbo,  -- ⭐ CAMPO AGREGADO
        v.motor, v.combustible, v.nroAirbags, v.seguridadChoques, v.organizacionSeguridad,
        v.anioFabricacion, v.confiabilidad, v.puntuacion, v.observaciones,
        CASE WHEN v.fotografia IS NOT NULL THEN 1 ELSE 0 END AS tieneFotografia,
        v.usuarioRegistro, v.fechaRegistro, v.estado
    FROM Vehiculo v
    INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
    INNER JOIN Marca m ON mv.idMarca = m.id
    INNER JOIN Procedencia p ON m.idProcedencia = p.id
    INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
    WHERE v.estado <> -1 AND p.descripcion = 'Japonés'
          AND (
            @parametro = '' 
            OR mv.nombre LIKE '%' + @parametro + '%'
            OR m.nombre LIKE '%' + @parametro + '%'
            OR v.generacion LIKE '%' + @parametro + '%'
            OR v.motor LIKE '%' + @parametro + '%'
            OR v.paisFabricacion LIKE '%' + @parametro + '%'
            OR v.anioFabricacion LIKE '%' + @parametro + '%'
            OR v.transmision LIKE '%' + @parametro + '%'
            OR v.combustible LIKE '%' + @parametro + '%'
            OR v.confiabilidad LIKE '%' + @parametro + '%'
            OR v.puntuacion LIKE '%' + @parametro + '%'
        )

    ORDER BY m.nombre ASC, mv.nombre ASC, v.anioFabricacion DESC;
END

-- =====================================================
-- PROCEDIMIENTO: Vehículos Americanos
-- =====================================================
GO
ALTER PROC paVehiculosAmericanos @parametro VARCHAR(100) = ''
AS
BEGIN
    SELECT 
        v.id, mv.nombre AS modelo, m.nombre AS marca, p.descripcion AS procedencia,
        tv.descripcion AS tipoVehiculo, v.generacion, v.paisFabricacion, 
        v.transmision, v.nroVelocidades, v.nroPuertas, v.nroAsientos, v.tieneTurbo,
        v.motor, v.combustible, v.nroAirbags, v.seguridadChoques, v.organizacionSeguridad,
        v.anioFabricacion, v.confiabilidad, v.puntuacion, v.observaciones,
        CASE WHEN v.fotografia IS NOT NULL THEN 1 ELSE 0 END AS tieneFotografia,
        v.usuarioRegistro, v.fechaRegistro, v.estado
    FROM Vehiculo v
    INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
    INNER JOIN Marca m ON mv.idMarca = m.id
    INNER JOIN Procedencia p ON m.idProcedencia = p.id
    INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
    WHERE v.estado <> -1 AND p.descripcion = 'Americano'
        AND (
            @parametro = '' 
            OR mv.nombre LIKE '%' + @parametro + '%'
            OR m.nombre LIKE '%' + @parametro + '%'
            OR v.generacion LIKE '%' + @parametro + '%'
            OR v.motor LIKE '%' + @parametro + '%'
            OR v.paisFabricacion LIKE '%' + @parametro + '%'
            OR v.anioFabricacion LIKE '%' + @parametro + '%'
            OR v.transmision LIKE '%' + @parametro + '%'
            OR v.combustible LIKE '%' + @parametro + '%'
            OR v.confiabilidad LIKE '%' + @parametro + '%'
            OR v.puntuacion LIKE '%' + @parametro + '%'
        )
    ORDER BY m.nombre ASC, mv.nombre ASC, v.anioFabricacion DESC;
END

-- =====================================================
-- PROCEDIMIENTO: Vehículos Chinos
-- =====================================================
GO
ALTER PROC paVehiculosChinos
    @parametro VARCHAR(100) = ''
AS
BEGIN
    SELECT 
        v.id, mv.nombre AS modelo, m.nombre AS marca, p.descripcion AS procedencia,
        tv.descripcion AS tipoVehiculo, v.generacion, v.paisFabricacion, 
        v.transmision, v.nroVelocidades, v.nroPuertas, v.nroAsientos, v.tieneTurbo,
        v.motor, v.combustible, v.nroAirbags, v.seguridadChoques, v.organizacionSeguridad,
        v.anioFabricacion, v.confiabilidad, v.puntuacion, v.observaciones,
        CASE WHEN v.fotografia IS NOT NULL THEN 1 ELSE 0 END AS tieneFotografia,
        v.usuarioRegistro, v.fechaRegistro, v.estado
    FROM Vehiculo v
    INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
    INNER JOIN Marca m ON mv.idMarca = m.id
    INNER JOIN Procedencia p ON m.idProcedencia = p.id
    INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
    WHERE v.estado <> -1 
        AND p.descripcion = 'Chino'
        AND (
            @parametro = '' 
            OR mv.nombre LIKE '%' + @parametro + '%'
            OR m.nombre LIKE '%' + @parametro + '%'
            OR v.generacion LIKE '%' + @parametro + '%'
            OR v.motor LIKE '%' + @parametro + '%'
            OR v.paisFabricacion LIKE '%' + @parametro + '%'
            OR v.anioFabricacion LIKE '%' + @parametro + '%'
            OR v.transmision LIKE '%' + @parametro + '%'
            OR v.combustible LIKE '%' + @parametro + '%'
            OR v.confiabilidad LIKE '%' + @parametro + '%'
            OR v.puntuacion LIKE '%' + @parametro + '%'
        )
    ORDER BY m.nombre ASC, mv.nombre ASC, v.anioFabricacion DESC;
END

-- =====================================================
-- PROCEDIMIENTO: Vehículos Alemanes
-- =====================================================
GO
ALTER PROC paVehiculosAlemanes
    @parametro VARCHAR(100) = ''
AS
BEGIN
    SELECT 
        v.id, 
        mv.nombre AS modelo, 
        m.nombre AS marca, 
        p.descripcion AS procedencia,
        tv.descripcion AS tipoVehiculo, 
        v.generacion, 
        v.paisFabricacion, 
        v.transmision, 
        v.nroVelocidades,
        v.nroPuertas,
        v.nroAsientos,
        v.tieneTurbo,
        v.motor, 
        v.combustible, 
        v.nroAirbags,
        v.seguridadChoques,
        v.organizacionSeguridad,
        v.anioFabricacion, 
        v.confiabilidad, 
        v.puntuacion,
        v.observaciones, 
          CASE WHEN v.fotografia IS NOT NULL THEN 1 ELSE 0 END AS tieneFotografia,
        v.usuarioRegistro, v.fechaRegistro, v.estado
    FROM Vehiculo v
    INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
    INNER JOIN Marca m ON mv.idMarca = m.id
    INNER JOIN Procedencia p ON m.idProcedencia = p.id
    INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
    WHERE v.estado <> -1 
        AND p.descripcion = 'Alemán'
         AND (
            @parametro = '' 
            OR mv.nombre LIKE '%' + @parametro + '%'
            OR m.nombre LIKE '%' + @parametro + '%'
            OR v.generacion LIKE '%' + @parametro + '%'
            OR v.motor LIKE '%' + @parametro + '%'
            OR v.paisFabricacion LIKE '%' + @parametro + '%'
            OR v.anioFabricacion LIKE '%' + @parametro + '%'
            OR v.transmision LIKE '%' + @parametro + '%'
            OR v.combustible LIKE '%' + @parametro + '%'
            OR v.confiabilidad LIKE '%' + @parametro + '%'
            OR v.puntuacion LIKE '%' + @parametro + '%'
        )
    ORDER BY m.nombre ASC, mv.nombre ASC, v.anioFabricacion DESC;
END

-- =====================================================
-- PROCEDIMIENTO: Vehículos Europeos
-- =====================================================
GO
ALTER PROC paVehiculosEuropeos
    @parametro VARCHAR(100) = ''
AS
BEGIN
    SELECT 
        v.id, 
        mv.nombre AS modelo, 
        m.nombre AS marca, 
        p.descripcion AS procedencia,
        tv.descripcion AS tipoVehiculo, 
        v.generacion, 
        v.paisFabricacion, 
        v.transmision, 
        v.nroVelocidades,
        v.nroPuertas,
        v.nroAsientos,
        v.tieneTurbo,
        v.motor, 
        v.combustible, 
        v.nroAirbags,
        v.seguridadChoques,
        v.organizacionSeguridad,
        v.anioFabricacion, 
        v.confiabilidad, 
        v.puntuacion,
        v.observaciones, 
         CASE WHEN v.fotografia IS NOT NULL THEN 1 ELSE 0 END AS tieneFotografia,
        v.usuarioRegistro, v.fechaRegistro, v.estado
    FROM Vehiculo v
    INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
    INNER JOIN Marca m ON mv.idMarca = m.id
    INNER JOIN Procedencia p ON m.idProcedencia = p.id
    INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
    WHERE v.estado <> -1 
        AND p.descripcion = 'Europeo'
         AND (
            @parametro = '' 
            OR mv.nombre LIKE '%' + @parametro + '%'
            OR m.nombre LIKE '%' + @parametro + '%'
            OR v.generacion LIKE '%' + @parametro + '%'
            OR v.motor LIKE '%' + @parametro + '%'
            OR v.paisFabricacion LIKE '%' + @parametro + '%'
            OR v.anioFabricacion LIKE '%' + @parametro + '%'
            OR v.transmision LIKE '%' + @parametro + '%'
            OR v.combustible LIKE '%' + @parametro + '%'
            OR v.confiabilidad LIKE '%' + @parametro + '%'
            OR v.puntuacion LIKE '%' + @parametro + '%'
        )
    ORDER BY m.nombre ASC, mv.nombre ASC, v.anioFabricacion DESC;
END


-- =====================================================
-- PROCEDIMIENTO: Vehículos Coreanos
-- =====================================================
GO
ALTER PROC paVehiculosCoreanos
    @parametro VARCHAR(100) = ''
AS
BEGIN
    SELECT 
        v.id, 
        mv.nombre AS modelo, 
        m.nombre AS marca, 
        p.descripcion AS procedencia,
        tv.descripcion AS tipoVehiculo, 
        v.generacion, 
        v.paisFabricacion, 
        v.transmision, 
        v.nroVelocidades,
        v.nroPuertas,
        v.nroAsientos,
        v.tieneTurbo,
        v.motor, 
        v.combustible, 
        v.nroAirbags,
        v.seguridadChoques,
        v.organizacionSeguridad,
        v.anioFabricacion, 
        v.confiabilidad, 
        v.puntuacion,
        v.observaciones, 
          CASE WHEN v.fotografia IS NOT NULL THEN 1 ELSE 0 END AS tieneFotografia,
        v.usuarioRegistro, v.fechaRegistro, v.estado
    FROM Vehiculo v
    INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
    INNER JOIN Marca m ON mv.idMarca = m.id
    INNER JOIN Procedencia p ON m.idProcedencia = p.id
    INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id
    WHERE v.estado <> -1 
        AND p.descripcion = 'Coreano'
         AND (
            @parametro = '' 
            OR mv.nombre LIKE '%' + @parametro + '%'
            OR m.nombre LIKE '%' + @parametro + '%'
            OR v.generacion LIKE '%' + @parametro + '%'
            OR v.motor LIKE '%' + @parametro + '%'
            OR v.paisFabricacion LIKE '%' + @parametro + '%'
            OR v.anioFabricacion LIKE '%' + @parametro + '%'
            OR v.transmision LIKE '%' + @parametro + '%'
            OR v.combustible LIKE '%' + @parametro + '%'
            OR v.confiabilidad LIKE '%' + @parametro + '%'
            OR v.puntuacion LIKE '%' + @parametro + '%'
        )
    ORDER BY m.nombre ASC, mv.nombre ASC, v.anioFabricacion DESC;
END

-- =====================================================
-- NUEVO: PROCEDIMIENTO PARA OBTENER SOLO LA FOTOGRAFÍA
-- =====================================================

GO
CREATE PROC paVehiculoObtenerFotografia
    @id INT
AS
BEGIN
    SELECT fotografia
    FROM Vehiculo
    WHERE id = @id AND estado != -1;
END


-- =====================================================
-- ACTUALIZAR: PROCEDIMIENTO DE INSERCIÓN
-- =====================================================
GO
ALTER PROC paVehiculoInsertar
    @nroChasis VARCHAR(17),
    @idModeloVehiculo INT,
    @generacion VARCHAR(50),
    @paisFabricacion VARCHAR(50),
    @transmision VARCHAR(20),
    @tieneTurbo BIT,
    @motor VARCHAR(50),
    @combustible VARCHAR(20),
    @nroAirbags INT,
    @seguridadChoques INT,
    @organizacionSeguridad VARCHAR(50),
    @anioFabricacion INT,
    @confiabilidad VARCHAR(10),
    @puntuacion DECIMAL(3,1),
    @kilometraje INT = NULL,
    @observaciones VARCHAR(500) = NULL,
    @fotografia VARBINARY(MAX) = NULL  -- ⭐ NUEVO PARÁMETRO
AS
    INSERT INTO Vehiculo (
        nroChasis, idModeloVehiculo, generacion, paisFabricacion, transmision,
        tieneTurbo, motor, combustible, nroAirbags, seguridadChoques, organizacionSeguridad,
        anioFabricacion, confiabilidad, puntuacion, kilometraje, observaciones, fotografia
    )
    VALUES (
        @nroChasis, @idModeloVehiculo, @generacion, @paisFabricacion, @transmision,
        @tieneTurbo, @motor, @combustible, @nroAirbags, @seguridadChoques, @organizacionSeguridad,
        @anioFabricacion, @confiabilidad, @puntuacion, @kilometraje, @observaciones, @fotografia
    );
    
    SELECT SCOPE_IDENTITY() AS nuevoId;

    -- =====================================================
-- ACTUALIZAR: PROCEDIMIENTO DE ACTUALIZACIÓN
-- =====================================================
GO
ALTER PROC paVehiculoActualizar
    @id INT,
    @generacion VARCHAR(50),
    @paisFabricacion VARCHAR(50),
    @transmision VARCHAR(20),
    @tieneTurbo BIT,
    @motor VARCHAR(50),
    @combustible VARCHAR(20),
    @nroAirbags INT,
    @seguridadChoques INT,
    @organizacionSeguridad VARCHAR(50),
    @anioFabricacion INT,
    @confiabilidad VARCHAR(10),
    @puntuacion DECIMAL(3,1),
    @kilometraje INT = NULL,
    @observaciones VARCHAR(500) = NULL,
    @fotografia VARBINARY(MAX) = NULL  -- ⭐ NUEVO PARÁMETRO
AS
    -- Guardar historial si cambia la confiabilidad
    IF EXISTS (SELECT 1 FROM Vehiculo WHERE id = @id AND confiabilidad != @confiabilidad)
        INSERT INTO HistorialConfiabilidad (idVehiculo, confiabilidadAnterior, confiabilidadNueva, 
                                           puntuacionAnterior, puntuacionNueva, motivo)
        SELECT @id, confiabilidad, @confiabilidad, puntuacion, @puntuacion, 
               'Actualización manual de confiabilidad'
        FROM Vehiculo WHERE id = @id;
    
    UPDATE Vehiculo 
    SET generacion = @generacion,
        paisFabricacion = @paisFabricacion,
        transmision = @transmision,
        tieneTurbo = @tieneTurbo,
        motor = @motor,
        combustible = @combustible,
        nroAirbags = @nroAirbags,
        seguridadChoques = @seguridadChoques,
        organizacionSeguridad = @organizacionSeguridad,
        anioFabricacion = @anioFabricacion,
        confiabilidad = @confiabilidad,
        puntuacion = @puntuacion,
        kilometraje = @kilometraje,
        observaciones = @observaciones,
        fotografia = ISNULL(@fotografia, fotografia)  -- ⭐ Solo actualiza si se proporciona
    WHERE id = @id;
    
    SELECT 'Actualizado correctamente' AS mensaje;




EXEC paVehiculosJaponeses ''

-- PROCEDIMIENTO ALMACENADO: Listar Empleados con Usuario
-- =====================================================
IF OBJECT_ID('paEmpleadoListar', 'P') IS NOT NULL
    DROP PROCEDURE paEmpleadoListar;
GO

CREATE PROCEDURE paEmpleadoListar
    @parametro VARCHAR(100) = ''
AS
BEGIN
    SELECT 
        e.id,
        e.cedulaIdentidad,
        e.nombres + ' ' + ISNULL(e.primerApellido, '') + ' ' + ISNULL(e.segundoApellido, '') AS nombreCompleto,
        c.descripcion AS cargo,
        e.celular,
        e.direccion,
        CASE WHEN EXISTS(SELECT 1 FROM Usuario u WHERE u.idEmpleado = e.id AND u.estado != -1) 
             THEN 'SÍ' 
             ELSE 'NO' 
        END AS tieneUsuario,
        e.fechaRegistro,
        e.usuarioRegistro
    FROM Empleado e
    INNER JOIN Cargo c ON e.idCargo = c.id
    WHERE e.estado != -1
        AND (
            @parametro = ''
            OR e.cedulaIdentidad LIKE '%' + @parametro + '%'
            OR e.nombres LIKE '%' + @parametro + '%'
            OR e.primerApellido LIKE '%' + @parametro + '%'
            OR e.segundoApellido LIKE '%' + @parametro + '%'
            OR c.descripcion LIKE '%' + @parametro + '%'
        )
    ORDER BY nombreCompleto ASC;
END
GO

PRINT 'Procedimiento paEmpleadoListar creado';



-- =====================================================
-- DATOS INICIALES
-- =====================================================

-- Cargos
INSERT INTO Cargo (descripcion) VALUES 
('Administrador Sistema'),
('Técnico Automotriz'),
('Evaluador Vehicular'),
('Consultor Ventas');

-- Empleados
INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, segundoApellido, direccion, celular, idCargo) VALUES 
('12345678', 'Kevin Eduardo', 'Cardenas', 'Mamani', 'Av. Venezuela #123', 71234567, 1),
('87654321', 'María José', 'Fernández', 'López', 'Calle Bolívar #456', 72345678, 2);

-- Usuarios
INSERT INTO Usuario (idEmpleado, usuario, clave, esAdministrador) VALUES 
(1, 'admin', 'YWRtaW4xMjM=', 1),
(2, 'maria.fernandez', 'bWFyaWExMjM=', 0);

-- Procedencias
INSERT INTO Procedencia (codigo, descripcion) VALUES 
('JPN', 'Japonés'),
('USA', 'Americano'),
('CHN', 'Chino'),
('GER', 'Alemán'),
('EUR', 'Europeo'),
('KOR', 'Coreano');

-- Marcas
INSERT INTO Marca (idProcedencia, nombre, descripcion) VALUES 
(1, 'Toyota', 'Fabricante japonés líder en confiabilidad'),
(1, 'Honda', 'Marca japonesa reconocida por durabilidad'),
(1, 'Nissan', 'Marca japonesa innovadora'),
(1, 'Suzuki', 'Fabricante japonés de vehículos compactos'),
(2, 'Ford', 'Fabricante americano histórico'),
(2, 'Chevrolet', 'Marca americana popular'),
(4, 'Volkswagen', 'Fabricante alemán de calidad'),
(6, 'Hyundai', 'Fabricante coreano líder');

-- Tipos de Vehículo
INSERT INTO TipoVehiculo (descripcion) VALUES 
('Sedán'),
('SUV'),
('Pickup'),
('Hatchback'),
('Coupé');

-- MODELOS SIMPLIFICADOS (Solo nombre)
INSERT INTO ModeloVehiculo (idMarca, idTipoVehiculo, nombre, descripcion) VALUES 
(1, 1, 'Corolla', 'Sedán compacto confiable'),
(1, 2, 'RAV4', 'SUV versátil y espaciosa'),
(2, 1, 'Civic', 'Sedán deportivo y eficiente'),
(2, 2, 'CR-V', 'SUV familiar confiable'),
(3, 4, 'Versa', 'Hatchback económico'),
(4, 4, 'Baleno', 'Hatchback compacto'),
(5, 3, 'F-150', 'Pickup robusta y potente'),
(7, 4, 'Golf', 'Hatchback premium'),
(8, 1, 'Elantra', 'Sedán moderno y equipado');

-- VEHÍCULOS COMPLETOS (Con TODAS las características)
INSERT INTO Vehiculo (
    nroChasis, idModeloVehiculo, generacion, paisFabricacion, transmision, tieneTurbo,
    motor, combustible, nroAirbags, seguridadChoques, organizacionSeguridad,
    anioFabricacion, confiabilidad, puntuacion, kilometraje, observaciones
) VALUES 
-- Toyota Corolla variantes
('1HGBH41JXMN109186', 1, 'Gen 12 (2020-2024)', 'Japón', 'CVT', 0, '2.0L Híbrido', 'Híbrido', 8, 5, 'JNCAP', 2022, 'Excelente', 9.5, 15000, 'Versión premium japonesa con máxima seguridad'),
('2T3RFREV8FW123456', 1, 'Gen 12 (2020-2024)', 'Brasil', 'Manual', 0, '1.8L', 'Gasolina', 6, 4, 'Latin NCAP', 2021, 'Bueno', 8.0, 45000, 'Versión brasileña con buena relación calidad-precio'),
('3FADP4BJ5NM789012', 1, 'Gen 11 (2012-2019)', 'México', 'CVT', 0, '1.8L', 'Gasolina', 6, 4, 'NHTSA', 2018, 'Bueno', 8.2, 78000, 'Generación anterior aún confiable'),

-- Nissan Versa - Diferencia por transmisión
('4S3BMHB68B3234567', 5, 'Gen 2 (2015-2020)', 'México', 'Manual', 0, '1.6L', 'Gasolina', 4, 3, 'Latin NCAP', 2019, 'Bueno', 7.8, 52000, 'Transmisión manual confiable'),
('5FRYD3H49HB345678', 5, 'Gen 2 (2015-2020)', 'México', 'CVT', 0, '1.6L', 'Gasolina', 4, 3, 'Latin NCAP', 2019, 'Regular', 5.5, 38000, 'CVT presenta problemas prematuros'),

-- Suzuki Baleno - Diferencia por país
('6T1BF1FK8GU456789', 6, 'Gen 2 (2015-actual)', 'India', 'Manual', 0, '1.2L', 'Gasolina', 2, 2, 'Global NCAP', 2020, 'Regular', 6.5, 35000, 'Versión india básica - 2 airbags'),
('7JTEBU5JR8K567890', 6, 'Gen 2 (2015-actual)', 'Hungría', 'Manual', 0, '1.2L', 'Gasolina', 6, 4, 'Euro NCAP', 2020, 'Bueno', 8.5, 28000, 'Versión europea - 6 airbags y mejor seguridad'),

-- Ford F-150 - Con y sin turbo
('8FTFW1ET5BFA67890', 7, 'Gen 14 (2021-actual)', 'Estados Unidos', 'Manual', 0, 'V8 5.0L', 'Gasolina', 6, 4, 'NHTSA', 2022, 'Excelente', 9.0, 12000, 'Motor V8 aspirado muy confiable'),
('9FTEW1E58BKE78901', 7, 'Gen 14 (2021-actual)', 'Estados Unidos', 'Automática', 1, '3.5L EcoBoost', 'Gasolina', 6, 4, 'NHTSA', 2022, 'Bueno', 7.5, 25000, 'Turbo tiene problemas ocasionales');


-- Establecer velocidades por defecto según tipo de transmisión
UPDATE Vehiculo
SET nroVelocidades = CASE 
    WHEN transmision = 'Manual' THEN 5
    WHEN transmision = 'Automática' THEN 6
    WHEN transmision = 'Semiautomática' THEN 6
    WHEN transmision = 'Doble Embrague' THEN 7
    WHEN transmision = 'CVT' THEN NULL
    ELSE 5
END
WHERE nroVelocidades IS NULL;

PRINT 'Datos existentes actualizados con valores por defecto';



PRINT '========================================';
PRINT 'CAMPO VELOCIDADES IMPLEMENTADO';
PRINT '========================================';
PRINT 'Reglas aplicadas:';
PRINT '  • Manual: 4-6 velocidades';
PRINT '  • Automática: 4-10 velocidades';
PRINT '  • CVT: NULL (sin velocidades)';
PRINT '  • Semiautomática/Doble Embrague: 6-8 vel.';
PRINT '========================================';

UPDATE Vehiculo
SET nroAsientos = CASE 
    WHEN idModeloVehiculo IN (SELECT id FROM ModeloVehiculo WHERE idTipoVehiculo = 3) THEN 5  -- Pickup: 5 asientos
    WHEN idModeloVehiculo IN (SELECT id FROM ModeloVehiculo WHERE idTipoVehiculo = 2) THEN 7  -- SUV: 7 asientos
    WHEN idModeloVehiculo IN (SELECT id FROM ModeloVehiculo WHERE idTipoVehiculo = 4) THEN 5  -- Hatchback: 5 asientos
    ELSE 5  -- Sedán y otros: 5 asientos por defecto
END
WHERE nroAsientos IS NULL;

UPDATE v
SET nroPuertas = CASE 
    WHEN tv.descripcion = 'Pickup' THEN 4       -- Pickups generalmente tienen 4 puertas
    WHEN tv.descripcion = 'Coupé' THEN 3        -- Coupés tienen 3 puertas
    WHEN tv.descripcion = 'Hatchback' THEN 5    -- Hatchbacks incluyen portón
    WHEN tv.descripcion = 'SUV' THEN 5          -- SUVs incluyen portón
    ELSE 5                                       -- Sedán y otros: 5 puertas por defecto
END
FROM Vehiculo v
INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
INNER JOIN TipoVehiculo tv ON mv.idTipoVehiculo = tv.id;


-- =====================================================
-- ÍNDICES PARA OPTIMIZACIÓN
-- =====================================================

CREATE NONCLUSTERED INDEX IX_Vehiculo_Estado ON Vehiculo(estado);
CREATE NONCLUSTERED INDEX IX_Vehiculo_Confiabilidad ON Vehiculo(confiabilidad);
CREATE NONCLUSTERED INDEX IX_Marca_Procedencia ON Marca(idProcedencia);
CREATE NONCLUSTERED INDEX IX_ModeloVehiculo_Marca ON ModeloVehiculo(idMarca);
CREATE NONCLUSTERED INDEX IX_Usuario_Estado ON Usuario(estado);


-- VERIFICAR Y CREAR ÍNDICES ADICIONALES
-- =====================================================

-- Índice para búsquedas por empleado en Usuario
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Usuario_IdEmpleado' AND object_id = OBJECT_ID('Usuario'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Usuario_IdEmpleado ON Usuario(idEmpleado) WHERE estado != -1;
    PRINT 'Índice IX_Usuario_IdEmpleado creado';
END

-- Índice para búsquedas por estado en Empleado
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Empleado_Estado' AND object_id = OBJECT_ID('Empleado'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Empleado_Estado ON Empleado(estado);
    PRINT 'Índice IX_Empleado_Estado creado';
END

-- Índice para búsquedas por cargo
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Empleado_IdCargo' AND object_id = OBJECT_ID('Empleado'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Empleado_IdCargo ON Empleado(idCargo);
    PRINT 'Índice IX_Empleado_IdCargo creado';
END

-- =====================================================
-- RESUMEN
-- =====================================================

PRINT '';
PRINT '';
PRINT 'ESTADÍSTICAS:';
SELECT 'Modelos registrados' AS Concepto, COUNT(*) AS Cantidad FROM ModeloVehiculo WHERE estado = 1
UNION ALL
SELECT 'Vehículos registrados', COUNT(*) FROM Vehiculo WHERE estado = 1
UNION ALL
SELECT 'Marcas disponibles', COUNT(*) FROM Marca WHERE estado = 1
UNION ALL
SELECT 'Usuarios creados', COUNT(*) FROM Usuario WHERE estado = 1;

SELECT COUNT(*) FROM Vehiculo v
INNER JOIN ModeloVehiculo mv ON v.idModeloVehiculo = mv.id
INNER JOIN Marca m ON mv.idMarca = m.id
INNER JOIN Procedencia p ON m.idProcedencia = p.id
WHERE p.descripcion = 'Japonés'

select * FROM Marca
SELECT * FROM Procedencia WHERE estado != -1;
SELECT * FROM Cargo



PRINT '========================================';
PRINT 'CAMPO NRO PUERTAS IMPLEMENTADO';
PRINT '========================================';
PRINT 'Valores permitidos: 3, 4, 5';
PRINT 'Valor por defecto: 5';
PRINT '========================================';

PRINT '';
PRINT 'USUARIOS:';
PRINT '   admin / admin123 (Administrador)';
PRINT '   maria.fernandez / maria123 (Usuario)';
PRINT '';
PRINT 'ESTRUCTURA:';
PRINT '   MODELO: Simple (solo nombre)';
PRINT '   VEHICULO: Completo (todas las características)';
PRINT '';
PRINT 'EJEMPLOS EN LA BD:';
PRINT '   • 3 Toyota Corollas diferentes';
PRINT '   • 2 Nissan Versas (Manual vs CVT)';
PRINT '   • 2 Suzuki Balenos (India vs Hungría)';
PRINT '   • 2 Ford F-150 (V8 vs Turbo)';
PRINT '';
PRINT '========================================';
