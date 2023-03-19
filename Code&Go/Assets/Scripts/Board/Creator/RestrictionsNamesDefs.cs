using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictionsNamesDefs : MonoBehaviour
{
    public static Dictionary<string, string> categoryNames = new Dictionary<string, string>
    {
        { "Comienzo", "start"},
        { "Variables", "variable"},
        { "Condiciones", "logic"},
        { "Bucles", "control"},
        { "Enteros", "math"},
        { "Texto", "text"},
        { "Acciones", "movement"},
        { "Funciones", "procedures"}
    };
    public static Dictionary<string, string> blocksNames = new Dictionary<string, string>
    {
        { "Comienzo", "start_start"},
        { "Numero", "math_number"},
        { "Aritmetica", "math_arithmetic"},
        { "Texto", "text"},
        { "Cambiar variable", "variables_set"},
        { "Variable", "variables_get"},
        { "Bucle", "controls_repeat"},
        { "Bucle condicion", "controls_whileUntil"},
        { "Movimiento", "movement_move"},
        { "Rotacion", "movement_rotate"},
        { "Estado", "movement_activate_door"},
        { "Intensidad", "movement_laser_change_intensity"},
        { "Funcion", "procedures_defnoreturn"},
        { "Negar", "logic_negate"},
        { "Celdas ocupadas", "logic_cells_occupied"},
        { "Comparar", "logic_compare"},
        { "Operacion", "logic_operation"},
        { "Booleano", "logic_boolean"},
        { "Si-entonces", "logic_if"},
        { "Si-entonces-no", "logic_ifelse"},
    };
}
