CREATE DATABASE tallerMecanico;
GO

USE tallerMecanico;
GO

CREATE TABLE clientes(
    claveCliente INT IDENTITY(1,1) PRIMARY KEY,
    rfc VARCHAR(13) UNIQUE NOT NULL,
    nombre VARCHAR(30) NOT NULL,
    apellidoPaterno VARCHAR(20) NOT NULL,
    apellidoMaterno VARCHAR(20) NOT NULL,
    domicilio VARCHAR(40) NOT NULL,
    colonia VARCHAR(30) NOT NULL,
    codigoPostal VARCHAR(5) NOT NULL,
    ciudad VARCHAR(30) NOT NULL,
    telefono VARCHAR(10) NOT NULL,
    telefono2 VARCHAR(10),
    correo VARCHAR(40),
    fechaRegistro DATE NOT NULL
);
GO

CREATE TABLE vehiculos(
    numeroSerie INT PRIMARY KEY,
    placas VARCHAR(9) NOT NULL,
    marca VARCHAR(20) NOT NULL,
    modelo VARCHAR(20) NOT NULL,
    anio int NOT NULL,
    color VARCHAR(20),
    kilometraje FLOAT NOT NULL,
    tipo VARCHAR(30) NOT NULL,
    antiguedad AS (YEAR(GETDATE()) - anio),
    claveCliente INT,
    CONSTRAINT FK_vehiculos_clientes
    FOREIGN KEY (claveCliente) REFERENCES clientes(claveCliente)
);
GO

CREATE TABLE mecanicos(
    numeroEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    rfc VARCHAR(13) UNIQUE NOT NULL,
    nombre VARCHAR(30) NOT NULL,
    apellidoPaterno VARCHAR(20) NOT NULL,
    apellidoMaterno VARCHAR(20) NOT NULL,
    especialidades VARCHAR(100) NOT NULL,
    telefono VARCHAR(10) NOT NULL,
    salario FLOAT NOT NULL,
    aniosExperiencia FLOAT
);
GO

CREATE TABLE servicios(
    claveServicio INT IDENTITY(1,1) PRIMARY KEY,
    nombreServicio VARCHAR(50) NOT NULL,
    descripcion VARCHAR(100) NOT NULL,
    costoBase FLOAT NOT NULL,
    tiempoEstimado TIME NOT NULL
);
GO

CREATE TABLE refacciones(
    codigoRefaccion INT PRIMARY KEY,
    nombre VARCHAR(30) NOT NULL,
    marca VARCHAR(20) NOT NULL,
    precioUnitario FLOAT NOT NULL,
    stockActual INT NOT NULL,
    stockMinimo INT NOT NULL,
    proveedor VARCHAR(40) NOT NULL
);
GO

CREATE TABLE ordenesServicio(
    folioOrden INT IDENTITY(1,1) PRIMARY KEY,
    fechaIngreso DATE NOT NULL,
    fechaEstimadaEntrega DATE NOT NULL,
    fechaRealEntrega DATE NOT NULL,
    estado VARCHAR(15) NOT NULL
    CHECK (estado IN ('Abierta','En proceso','Finalizada','Cancelada')),
    costoTotal FLOAT NOT NULL,
    numeroSerie INT,
    CONSTRAINT FK_ordenes_vehiculos
    FOREIGN KEY(numeroSerie) REFERENCES vehiculos(numeroSerie)
);
GO

CREATE TABLE OrdenServicio(
    folioOrden INT,
    claveServicio INT,
    CONSTRAINT PK_OrdenServicio PRIMARY KEY (folioOrden, claveServicio),
    CONSTRAINT FK_OS_orden
    FOREIGN KEY(folioOrden) REFERENCES ordenesServicio(folioOrden),
    CONSTRAINT FK_OS_servicio
    FOREIGN KEY(claveServicio) REFERENCES servicios(claveServicio)
);
GO

CREATE TABLE OrdenRefaccion(
    folioOrden INT,
    codigoRefaccion INT,
    CONSTRAINT PK_OrdenRefaccion PRIMARY KEY (folioOrden, codigoRefaccion),
    CONSTRAINT FK_OR_orden
        FOREIGN KEY(folioOrden) REFERENCES ordenesServicio(folioOrden),
    CONSTRAINT FK_OR_refaccion
        FOREIGN KEY(codigoRefaccion) REFERENCES refacciones(codigoRefaccion)
);
GO

CREATE TABLE MecanicoOrden(
    folioOrden INT,
    numeroEmpleado INT,
    CONSTRAINT PK_MecanicoOrden PRIMARY KEY (folioOrden, numeroEmpleado),
    CONSTRAINT FK_MO_orden
        FOREIGN KEY(folioOrden) REFERENCES ordenesServicio(folioOrden),
    CONSTRAINT FK_MO_mecanico
        FOREIGN KEY(numeroEmpleado) REFERENCES mecanicos(numeroEmpleado)
);
GO

INSERT INTO clientes(rfc, nombre, apellidoPaterno, apellidoMaterno, domicilio, colonia, codigoPostal, ciudad, telefono, correo, fechaRegistro) VALUES
('SAAD0508311H0', 'Daniel', 'Sanchez', 'Malagon', 'Durango #30', 'Centro', '38800', 'Moroleon', '4451234561', 'daniel@gmail.com', CAST(GETDATE() AS DATE)),
('VIAD0612201H4', 'Diego', 'Villagomez', 'Aguilera', 'Sonora #12', 'Deseada de arriba', '38982', 'Uriangato', '4459083456', 'diego@gmail.com', CAST(GETDATE() AS DATE)),
('ALMJ0401051H5', 'Jonathan', 'Alcantar', 'Malagon', 'Sonora #05', 'Desada de arriba', '38982', 'Uriangato', '4451051178', 'joathan@gmail.com', CAST(GETDATE() AS DATE)),
('COGA0625061H3', 'Alan Fransisco', 'Contreras', 'Guzman', 'Coahuila #24', 'Centro', '38900', 'Uriangato', '4456821398', 'alan@gmail.com', CAST(GETDATE() AS DATE)),
('RAZO0511121H2', 'Obed Isai', 'Ramirez', 'Zavala', 'Pipila #34', 'Centro', '38800', 'Moroleon', '4532156890', 'obed@gmail.com', CAST(GETDATE() AS DATE));
GO

INSERT INTO vehiculos (numeroSerie, placas, marca, modelo, anio, color, kilometraje, tipo, claveCliente) VALUES
(3112, 'ABC-123-D', 'Jeep', 'Rubicon', '2013', 'Blanco', 20000, 'Pickup', 3),
(2121, 'NAV-121-A', 'Cooper', 'Mini cooper', '2024', 'Gris', 21000, 'Auto compacto', 4),
(1234, 'TVI-315-M', 'Ford', 'Mustang', '2026', 'Negro', 24000, 'Deportivo', 1),
(4536, 'DAG-112-N', 'Ford', 'Raptor 4x4', '2018', 'Blanco', 20000, 'Camioneta', 2),
(4569, 'ALG-223-C', 'Chevrolet', 'Camaro', '2023', 'Rojo', 21000, 'Deportivo', 5);
GO

INSERT INTO mecanicos (rfc, nombre, apellidoPaterno, apellidoMaterno, especialidades, telefono, salario, aniosExperiencia) VALUES
('HUMI050107HGT', 'Isaac', 'Hurtado', 'Morales', 'Todo', '4451234568', 6000, 8),
('FORA051230HGT', 'Alan', 'Fonseca', 'Rios', 'Automotriz', '4451234568', 8000, 6),
('QWER051009HGT', 'Natanael', 'Cano', 'Hernandez', 'Todo', '4456890459', 6000, 4),
('FGHT991203HGT', 'Hassan', 'Fernandez', 'Gonzales', 'Todo', '4451239865', 3500, 5),
('NOZL051106HGT', 'Leonardo', 'Nuñez', 'Zavala', 'Hidraulico', '4451238956', 9000, 41);
GO

INSERT INTO servicios (nombreServicio, descripcion, costoBase, tiempoEstimado) VALUES
('Cambio de aceite', 'Cambio de aceite de motor y revisión de niveles', 450, '00:30:00'),
('Alineación y balanceo', 'Ajuste de dirección y balanceo de llantas', 600, '01:00:00'),
('Cambio de batería', 'Sustitución de batería y verificación del sistema eléctrico', 700, '00:40:00'),
('Cambio de amortiguadores', 'Sustitución de amortiguadores delanteros', 1800, '03:00:00'),
('Revisión de suspensión', 'Inspección completa del sistema de suspensión', 650, '01:15:00');
GO

INSERT INTO refacciones VALUES
(1001,'Filtro de aceite','ACDelco',120,25,5,'AutoZone'),
(1002,'Filtro de aire','Bosch',180,20,5,'Refaccionaria Lopez'),
(1003,'Bujía estándar','NGK',95,40,10,'Refaccionaria Central'),
(1004,'Batería 12V','LTH',1800,8,2,'LTH Distribuidor'),
(1005,'Pastillas de freno','Brembo',650,15,4,'Frenos MX'),
(1006,'Disco de freno','Brembo',950,10,3,'Frenos MX'),
(1007,'Amortiguador delantero','Monroe',1250,6,2,'Suspensiones Pro'),
(1008,'Aceite 5W30','Mobil',230,50,10,'Lubricantes MX'),
(1009,'Aceite 10W40','Castrol',220,45,10,'Lubricantes MX'),
(1010,'Filtro de gasolina','Bosch',210,18,5,'AutoRefacciones Diaz'),
(1011,'Banda de distribución','Gates',980,7,2,'Bandas y Mangueras SA'),
(1012,'Radiador','Denso',2400,3,1,'Sistemas Enfriamiento'),
(1013,'Termostato','Motorad',350,12,3,'Refaccionaria Central'),
(1014,'Bomba de agua','Aisin',1100,5,2,'Bombas Industriales'),
(1015,'Balatas traseras','Akebono',620,14,4,'Frenos MX');
GO

INSERT INTO ordenesServicio (fechaIngreso, fechaEstimadaEntrega, fechaRealEntrega, estado, costoTotal, numeroSerie) VALUES
('2026-02-01', '2026-02-02', '2026-02-02', 'Finalizada', 1050, 3112),
('2026-02-03', '2026-02-05', '2026-02-05', 'Finalizada', 2400, 2121),
('2026-02-04', '2026-02-06', '2026-02-06', 'Finalizada', 850, 1234),
('2026-02-05', '2026-02-07', '2026-02-07', 'Finalizada', 1800, 4536),
('2026-02-06', '2026-02-08', '2026-02-08', 'Finalizada', 650, 4569),
('2026-02-07', '2026-02-09', '2026-02-09', 'Finalizada', 1300, 3112),
('2026-02-08', '2026-02-10', '2026-02-10', 'Finalizada', 2700, 2121),
('2026-02-09', '2026-02-11', '2026-02-11', 'Finalizada', 1500, 4536);
GO

INSERT INTO OrdenServicio (folioOrden, claveServicio) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 1),
(7, 2),
(8, 4);
GO

INSERT INTO OrdenRefaccion (folioOrden, codigoRefaccion) VALUES
(1, 1001),
(2, 1004),
(3, 1005),
(4, 1007),
(5, 1013),
(6, 1008),
(7, 1011),
(8, 1015);
GO

INSERT INTO MecanicoOrden (folioOrden, numeroEmpleado) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 1),
(7, 2),
(8, 3);
GO

USE [master]
GO

-- 1. Si el usuario YA existe, lo borramos para crearlo limpio
IF EXISTS (SELECT * FROM sys.server_principals WHERE name = N'dafmon')
BEGIN
    DROP LOGIN [dafmon]
END
GO

-- 2. CREAMOS el usuario con la contraseña exacta de tu Web.config
CREATE LOGIN [dafmon]
    WITH PASSWORD=N'',
    DEFAULT_DATABASE=[tallerMecanico],
    CHECK_EXPIRATION=OFF,
    CHECK_POLICY=OFF
GO

-- 3. Entramos a tu base de datos y le damos permisos
USE [TallerMecanico]
GO

-- Si existe un usuario "huérfano" con ese nombre, lo borramos
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'dafmon')
BEGIN
    DROP USER [dafmon]
END
GO

-- Creamos el usuario en la base de datos
CREATE USER [dafmon] FOR LOGIN [dafmon]
GO

-- Le damos permisos de dueño (puede hacer todo)
ALTER ROLE [db_owner] ADD MEMBER [dafmon]
GO