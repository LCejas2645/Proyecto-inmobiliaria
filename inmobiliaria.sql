-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 19-04-2024 a las 01:06:35
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `auditoria`
--

CREATE TABLE `auditoria` (
  `id` int(11) NOT NULL,
  `idUsuario` int(11) NOT NULL,
  `idEntidad` int(11) NOT NULL,
  `entidad` tinyint(1) NOT NULL,
  `fechaAccion` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `auditoria`
--

INSERT INTO `auditoria` (`id`, `idUsuario`, `idEntidad`, `entidad`, `fechaAccion`) VALUES
(1, 16, 4, 1, '2024-04-09'),
(2, 16, 4, 1, '2024-04-09'),
(3, 19, 2, 0, '2024-04-19'),
(4, 11, 6, 1, '2024-04-30'),
(5, 19, 2, 0, '2024-04-19'),
(6, 11, 6, 1, '2024-04-30'),
(7, 11, 56, 1, '2024-04-17'),
(8, 16, 57, 1, '2024-04-18'),
(9, 16, 58, 1, '2024-04-18'),
(10, 16, 59, 1, '2024-04-18'),
(11, 16, 60, 1, '2024-04-18');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `id` int(11) NOT NULL,
  `idInquilino` int(11) NOT NULL,
  `idInmueble` int(11) NOT NULL,
  `idPropietario` int(11) NOT NULL,
  `idUsuario` int(11) DEFAULT NULL,
  `fechaInicio` date NOT NULL,
  `fechaFin` date NOT NULL,
  `vigente` tinyint(1) NOT NULL DEFAULT 1,
  `montoMensual` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id`, `idInquilino`, `idInmueble`, `idPropietario`, `idUsuario`, `fechaInicio`, `fechaFin`, `vigente`, `montoMensual`) VALUES
(61, 16, 36, 1, 16, '2024-04-18', '2024-10-09', 1, 120.0005),
(62, 24, 37, 31, 16, '2024-05-03', '2024-05-11', 1, 120.0005),
(63, 18, 38, 29, 16, '2024-03-31', '2024-10-06', 1, 150.00025),
(64, 23, 40, 1, 16, '2024-04-13', '2024-11-05', 1, 220.0005),
(65, 19, 42, 28, 16, '2024-03-31', '2024-05-17', 1, 220000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id` int(11) NOT NULL,
  `direccion` varchar(100) NOT NULL,
  `ambientes` int(11) NOT NULL,
  `superficie` int(11) NOT NULL,
  `latitud` double NOT NULL,
  `longitud` double NOT NULL,
  `idPropietario` int(11) NOT NULL,
  `disponible` tinyint(1) NOT NULL DEFAULT 1,
  `idTipo` int(11) NOT NULL,
  `uso` varchar(50) NOT NULL,
  `precio` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `direccion`, `ambientes`, `superficie`, `latitud`, `longitud`, `idPropietario`, `disponible`, `idTipo`, `uso`, `precio`) VALUES
(36, 'Av. Corrientes 123', 3, 100, -34.6037, -58.3816, 1, 1, 1, 'Comercial', 120.0005),
(37, 'Calle San Martín 456', 4, 200, -34.6118, -58.4173, 31, 1, 3, 'Residencial', 250.00075),
(38, 'Av. Rivadavia 789', 2, 80, -34.6037, -58.3816, 29, 1, 4, 'Residencial', 150.00025),
(39, 'Calle Florida 101', 1, 50, -34.6083, -58.3732, 30, 1, 1, 'Comercial', 80.001),
(40, 'Av. Santa Fe 543', 3, 120, -34.5952, -58.4026, 1, 1, 4, 'Residencial', 220.0005),
(41, 'calle123', 2, 111, 1111, 1111, 1, 1, 1, 'Comercial', 50000),
(42, 'Calle Lavalle 987', 2, 70, -34.6041, -58.3725, 28, 1, 1, 'Comercial', 100.0002),
(43, 'Av. Córdoba 210', 5, 250, -34.5986, -58.3764, 1, 1, 3, 'Residencial', 320000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id` int(11) NOT NULL,
  `nombre` varchar(255) DEFAULT NULL,
  `apellido` varchar(255) DEFAULT NULL,
  `dni` varchar(70) DEFAULT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id`, `nombre`, `apellido`, `dni`, `telefono`, `email`) VALUES
(16, 'Laura', ' García', '78901234', '7788990011', 'lauragarcia@example.com'),
(17, 'Diego', 'Fernández', ' 89012345', '8899001122', 'diegofernandez@example.com'),
(18, 'Sandra', 'López', '90123456', '9900112233', 'sandralopez@example.com'),
(19, 'Javier', 'Ruiz', '01234567', '0011223344', 'javierruiz@example.com'),
(20, 'Lucía', 'Sánchez', '12345678', '1122334455', 'luciasanchez@example.com'),
(21, 'Daniel', 'Martín', '23456789', '2233445566', 'danielmartin@example.com'),
(22, 'Sofía', 'Pérez', '34567890', '3344556677', 'sofiaperez@example.com'),
(23, 'Alejandro', 'Gómez', '45678901', '4455667788', 'alejandrogomez@example.com'),
(24, 'Andrea', 'Rodríguez', '56789012', '', '');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `numeroPago` int(11) NOT NULL,
  `idContrato` int(11) NOT NULL,
  `idUsuario` int(11) NOT NULL,
  `fecha` date NOT NULL,
  `importe` double NOT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1,
  `detalle` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `numeroPago`, `idContrato`, `idUsuario`, `fecha`, `importe`, `estado`, `detalle`) VALUES
(37, 1, 64, 16, '2024-01-01', 220.0005, 1, 'enero'),
(38, 2, 64, 16, '2024-02-10', 220.0005, 1, 'febrero'),
(39, 3, 64, 16, '2024-03-06', 220.0005, 1, 'marzo');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id` int(11) NOT NULL,
  `nombre` varchar(255) DEFAULT NULL,
  `apellido` varchar(255) DEFAULT NULL,
  `dni` varchar(20) DEFAULT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id`, `nombre`, `apellido`, `dni`, `telefono`, `email`) VALUES
(1, 'Juan', 'López', '12345678', '5551234567', 'juan@example.com'),
(2, 'María', 'García', '87654321', '5559876543', 'maria@example.com'),
(28, 'Daniel', 'Rodríguez', '34567890', '3344556677', 'danielrodriguez@example.com'),
(29, ' Ana', 'López', '45678901', '4455667788', 'analopez@example.com'),
(30, 'Marta', ' Martínez', '56789012', '5566778899', 'martamartinez@example.com'),
(31, 'Pablo', 'Sánchez', '67890123', '6677889900', 'pablosanchez@example.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo`
--

CREATE TABLE `tipo` (
  `id` int(11) NOT NULL,
  `tipoInmueble` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo`
--

INSERT INTO `tipo` (`id`, `tipoInmueble`) VALUES
(1, 'Local'),
(2, 'Depósito'),
(3, 'Casa'),
(4, 'Departamento'),
(6, 'Duplex');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(60) NOT NULL,
  `email` varchar(50) NOT NULL,
  `password` varchar(100) NOT NULL,
  `rol` int(11) NOT NULL,
  `avatarUrl` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `nombre`, `apellido`, `email`, `password`, `rol`, `avatarUrl`) VALUES
(10, 'empleado', 'j', 'empleado@gmail.com', 'DF4003F282A36710426874095B583ED7', 2, ''),
(11, 'Admin', 'admin', 'administrador@gmail.com', 'DF4003F282A36710426874095B583ED7', 1, '/uploads/05193db9-0779-4892-8e84-0e9df73ca200_FGcsfOhXEAI23FO.jpg'),
(16, 'Tamara', 'Abarza', 'abarzatamara6@gmail.com', 'DF4003F282A36710426874095B583ED7', 1, '/uploads/1fc0f500-0d1d-4bc1-8310-f7a59cee6e5b_images.jpeg'),
(19, 'Frank', 'Grimes', 'frankgrimes@email.com', 'DF4003F282A36710426874095B583ED7', 2, '/uploads/b048a404-9754-4cd7-907b-12b69c3bfd7b_Frank_Grimes.jpg');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idUsuario` (`idUsuario`);

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idInmueble` (`idInmueble`),
  ADD KEY `idInquilino` (`idInquilino`),
  ADD KEY `idPropietario` (`idPropietario`),
  ADD KEY `idUsuario` (`idUsuario`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idPropietario` (`idPropietario`),
  ADD KEY `idTipo` (`idTipo`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idContrato` (`idContrato`),
  ADD KEY `idUsuario` (`idUsuario`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `tipo`
--
ALTER TABLE `tipo`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `auditoria`
--
ALTER TABLE `auditoria`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=66;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=44;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=40;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;

--
-- AUTO_INCREMENT de la tabla `tipo`
--
ALTER TABLE `tipo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `auditoria`
--
ALTER TABLE `auditoria`
  ADD CONSTRAINT `auditoria_ibfk_1` FOREIGN KEY (`idUsuario`) REFERENCES `usuario` (`id`);

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`idInmueble`) REFERENCES `inmueble` (`id`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`idInquilino`) REFERENCES `inquilino` (`id`),
  ADD CONSTRAINT `contrato_ibfk_3` FOREIGN KEY (`idPropietario`) REFERENCES `propietario` (`id`),
  ADD CONSTRAINT `contrato_ibfk_4` FOREIGN KEY (`idUsuario`) REFERENCES `usuario` (`id`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`idPropietario`) REFERENCES `propietario` (`id`),
  ADD CONSTRAINT `inmueble_ibfk_2` FOREIGN KEY (`idTipo`) REFERENCES `tipo` (`id`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`idContrato`) REFERENCES `contrato` (`id`),
  ADD CONSTRAINT `pago_ibfk_2` FOREIGN KEY (`idUsuario`) REFERENCES `usuario` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
