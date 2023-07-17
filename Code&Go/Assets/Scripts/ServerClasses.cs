using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerClasses : MonoBehaviour
{
    [System.Serializable]
    public class Login
    {
        public string username;
        public string password;
    }
    [System.Serializable]
    public class LoginResponse
    {
        public string token;
    }
    [System.Serializable]
    public class Teacher
    {
        public string username;
        public bool enabled;
        public string rol;
        public int id;
    }
    [System.Serializable]
    public class Clase
    {
        public string description;
        public List<Teacher> teachers;
        public int students;
        public string name;
        public int id;
    }
    [System.Serializable]
    public class ClaseList
    {
        public List<Clase> list;
    }
}
