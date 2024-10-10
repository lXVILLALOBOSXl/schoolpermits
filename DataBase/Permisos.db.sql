BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS "estadoPermiso" (
    "idEstadoPermiso"	INTEGER NOT NULL,
    "descripcionEstado"	VARCHAR(30) NOT NULL,
    PRIMARY KEY("idEstadoPermiso" AUTOINCREMENT)
);

CREATE TABLE IF NOT EXISTS "tipoPermiso" (
    "idTipoPermiso"	INTEGER NOT NULL,
    "descripcionPermiso"	VARCHAR(30) NOT NULL,
    PRIMARY KEY("idTipoPermiso" AUTOINCREMENT)
);

CREATE TABLE IF NOT EXISTS "areaTrabajo" (
    "idAreaEmpleado"	INTEGER NOT NULL,
    "descripcionArea"	VARCHAR(40) NOT NULL,
    PRIMARY KEY("idAreaEmpleado" AUTOINCREMENT)
);

CREATE TABLE IF NOT EXISTS "tipoEmpleado" (
    "idTipoEmpleado"	INTEGER NOT NULL,
    "descripcionEmpleado"	VARCHAR(30) NOT NULL,
    PRIMARY KEY("idTipoEmpleado" AUTOINCREMENT)
);

CREATE TABLE IF NOT EXISTS "plantel" (
    "idTipoPlantel"	INTEGER NOT NULL,
    "descripcionPlantel"	VARCHAR(30) NOT NULL,
    PRIMARY KEY("idTipoPlantel" AUTOINCREMENT)
);

INSERT INTO "estadoPermiso" ("idEstadoPermiso","descripcionEstado") VALUES 
(1,'Aceptado'),
(2,'Rechazado'),
(3,'Solicitado'),
(4,'Aprobado (Supervisor)'),
(5,'Cancelado');

INSERT INTO "tipoPermiso" ("idTipoPermiso","descripcionPermiso") VALUES 
(1,'Economico'),
(2,'Cumpleaños'),
(3,'Dos Horas');

INSERT INTO "areaTrabajo" ("idAreaEmpleado","descripcionArea") VALUES 
(1,'Gastronomía'),
(2,'Arquitectura'),
(3,'Civil'),
(4,'Electrónica'),
(5,'Electromecánica'),
(6,'Industrial'),
(7,'Gestión Empresarial'),
(8,'Sistemas Computacionales');

INSERT INTO "tipoEmpleado" ("idTipoEmpleado","descripcionEmpleado") VALUES 
(1,'Director'),
(2,'Supervisor'),
(3,'Supervisor-Profesor'),
(4,'Profesor');

INSERT INTO "plantel" ("idTipoPlantel","descripcionPlantel") VALUES 
(1,'Arandas'),
(2,'Chapala'),
(3,'Cocula'),
(4,'El Grullo'),
(5,'La Huerta'),
(6,'Lagos de Moreno'),
(7,'Mascota'),
(8,'Puerto Vallarta'),
(9,'Tala'),
(10,'Tamazula'),
(11,'Tequila'),
(12,'Zapopan'),
(13,'Zapotlanejo');

CREATE TABLE IF NOT EXISTS "empleado" (
    "numeroDeNomina"	INTEGER NOT NULL,
    "nombres"	VARCHAR(30) NOT NULL,
    "apellidoPaterno"	VARCHAR(30) NOT NULL,
    "apellidoMaterno"	VARCHAR(30) NOT NULL,
    "curp"	VARCHAR(18) NOT NULL,
    "email"	VARCHAR(50) NOT NULL,
    "numeroTelefonico"	INTEGER NOT NULL,
    "calle"	VARCHAR(30) NOT NULL,
    "numeroInterior"	INTEGER,
    "numeroExterior"	INTEGER NOT NULL,
    "codigoPostal"	INTEGER NOT NULL,
    "colonia"	VARCHAR(30) NOT NULL,
    "municipio"	VARCHAR(30) NOT NULL,
    "fechaCumpleanos"	DATE NOT NULL,
    "fechaIngreso"	DATE NOT NULL,
    "areaTrabajo"	INTEGER NOT NULL,
    "tipoEmpleado"	INTEGER NOT NULL,
    "plantel"	INTEGER NOT NULL,
    FOREIGN KEY("areaTrabajo") REFERENCES "areaTrabajo"("idAreaEmpleado"),
    FOREIGN KEY("tipoEmpleado") REFERENCES "tipoEmpleado"("idTipoEmpleado"),
    FOREIGN KEY("plantel") REFERENCES "plantel"("idTipoPlantel"),
    PRIMARY KEY("numeroDeNomina" AUTOINCREMENT)
);

CREATE TABLE IF NOT EXISTS "Login" (
    "idLogin"	INTEGER NOT NULL,
    "password"	VARCHAR(30) NOT NULL,
    "usuario"	INTEGER NOT NULL,
    FOREIGN KEY("usuario") REFERENCES "empleado"("numeroDeNomina"),
    PRIMARY KEY("idLogin" AUTOINCREMENT)
);

CREATE TABLE IF NOT EXISTS "permiso" (
    "folio"	INTEGER NOT NULL,
    "empleado"	INTEGER NOT NULL,
    "fechaElaboracion"	DATE NOT NULL,
    "fechaJustificacionInicio"	DATE NOT NULL,
    "fechaJustificacionFin"	DATE NOT NULL,
    "horaInicio"	TIME,
    "horaFin"	TIME,
    "estadoPermiso"	INTEGER NOT NULL,
    "tipoPermiso"	INTEGER NOT NULL,
    FOREIGN KEY("empleado") REFERENCES "empleado"("numeroDeNomina"),
    FOREIGN KEY("estadoPermiso") REFERENCES "estadoPermiso"("idEstadoPermiso"),
    FOREIGN KEY("tipoPermiso") REFERENCES "tipoPermiso"("idTipoPermiso"),
    PRIMARY KEY("folio" AUTOINCREMENT)
);

INSERT INTO "empleado" ("numeroDeNomina","nombres","apellidoPaterno","apellidoMaterno","curp","email","numeroTelefonico","calle","numeroInterior","numeroExterior","codigoPostal","colonia","municipio","fechaCumpleanos","fechaIngreso","areaTrabajo","tipoEmpleado","plantel") VALUES 
(1001,'Yesenia Guadalupe','Gomez','Gonzalez','GOGY901103MJCMNS18','yeseniagomez@zapopan.tecmm.edu.mx',3398013978,'Sevilla',NULL,979,44280,'El Retiro','Guadalajara','03/11/1990','2019-03-03',5,4,1);

INSERT INTO "Login" ("idLogin","password","usuario") VALUES 
(1,'123456',1001);

COMMIT;