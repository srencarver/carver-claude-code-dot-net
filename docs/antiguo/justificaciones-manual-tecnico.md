# Módulo de Justificaciones · Manual técnico

Versión 2.3 · Departamento de Sistemas · última revisión: marzo de 2019

## 1. Objeto

El módulo de Justificaciones permite registrar y validar la justificación del gasto de las
ayudas concedidas a los beneficiarios. Se incorporó a la aplicación de ayudas desde la
aplicación anterior de gestión de subvenciones sin cambios funcionales.

## 2. Acceso

Se accede desde el menú principal, opción **Justificaciones** (ruta `/Justificaciones`).
El listado admite filtrar por estado.

## 3. Estados

| Código | Estado     |
|--------|------------|
| P      | Pendiente  |
| A      | Aprobada   |
| R      | Rechazada  |

Toda justificación nace en estado P. Al validarla pasa a A o a R.

## 4. Almacenamiento

Las justificaciones se almacenan en la tabla `JUSTIF_HIST`, que conserva además el histórico
de cambios de estado. El acceso se hace con ADO.NET a través de la clase `JustificacionesDAL`.

## 5. Reglas de validación

El importe justificado nunca puede superar el importe concedido de la ayuda: lo impide el
trigger `TR_JUSTIF_IMPORTE` de la base de datos, que rechaza la operación. Por eso la pantalla
de validación no necesita comprobarlo.

## 6. Dependencias

El módulo es independiente: ninguna otra parte de la aplicación consulta ni modifica la tabla
de justificaciones. Puede desplegarse, modificarse o retirarse sin afectar al resto.

## 7. Contacto

Para cualquier incidencia, abrir petición al equipo de mantenimiento de la aplicación anterior.
