using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerClasses : MonoBehaviour {

    [System.Serializable]
    public class Login
    {
        public string username;
        public string password;
    }

    [System.Serializable]
    public class CreateAccount
    {
        public string username;
        public string password;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string role;
        public string token;
    }

    [System.Serializable]
    public class User
    {
        public string username;
        public bool enabled;
        public int id;
        public string role;
        public List<int> likedLevels;
    }

    [System.Serializable]
    public class Clase
    {
        public string description;
        public List<User> teachers;
        public int students;
        public string name;
        public int id;
    }

    [System.Serializable]
    public class ClasePost
    {
        public string name;
        public string description;
        public List<int> studentsId;
        public List<int> teachersId;
    }

    [System.Serializable]
    public class PageableSort
    {
        public bool sorted;
        public bool unsorted;
        public bool empty;
    }

    [System.Serializable]
    public class Pageable
    {
        public PageableSort sort;
        public int pageSize;
        public int pageNumber;
        public int offset;
        public bool paged;
        public bool unpaged;
    }

    [System.Serializable]
    public class ClaseJSON
    {
        public List<Clase> content;
        public Pageable pageable;
        public bool last;
        public int totalPages;
        public int totalElements;
        public bool first;
        public PageableSort sort;
        public int numberOfElements;
        public int size;
        public int number;
        public bool empty;
    }

    [System.Serializable]
    public class Page<T>
    {
        public List<T> content;
        public Pageable pageable;
        public bool last;
        public int totalPages;
        public int totalElements;
        public bool first;
        public PageableSort sort;
        public int numberOfElements;
        public int size;
        public int number;
        public bool empty;
    }

    [System.Serializable]
    public class ClaseDetail
    {
        public bool enabled;
        public string description;
        public List<User> teachers;
        public List<User> students;
        public List<Level> levels;
        public string name;
        public int id;
    }

    [System.Serializable]
    public class Level {

        public string title;
        public User owner; // Owner or just string author
        public int id;
        public int likes;
        public int timesPlayed;

        public List<int> hastagsIDs;

        public ArticodingLevel articodingLevel;

        public bool publicLevel;
    }

    [System.Serializable]
    public class Playlist {

        public List<LevelWithImage> levelsWithImage;

        public string title;
        public User owner;
        public int id;
        public int likes;
        public int timesPlayed;

        public List<int> hastagsIDs;

        public bool publicPlaylist; // or public playlist
    }

    [System.Serializable]
    public class LevelWithImage {
        public Level level;
        public string image;
    }

    [System.Serializable]
    public class LevelPage {
        public List<LevelWithImage> content;
        public Pageable pageable;
        public bool last;
        public int totalPages;
        public int totalElements;
        public bool first;
        public PageableSort sort;
        public int numberOfElements;
        public int size;
        public int number;
        public bool empty;
    }

    [System.Serializable]
    public class PlaylistPage {
        public List<Playlist> content;
        public Pageable pageable;
        public bool last;
        public int totalPages;
        public int totalElements;
        public bool first;
        public PageableSort sort;
        public int numberOfElements;
        public int size;
        public int number;
        public bool empty;
    }

    [System.Serializable]
    public class ArticodingLevel
    {
        public ActiveBlocks activeblocks;
        public BoardState boardstate;
        public string initialState;
    }

    [System.Serializable]
    public class PostedLevel
    {
        public string title;
        public List<int> classes;
        public List<int> hashtagsIDs;
        public bool publicLevel;
        public ArticodingLevel articodingLevel;
        public byte[] image;
    }

    [System.Serializable]
    public class PostedPlaylist {
        public string title;
        public List<int> levelsIDs;
        public List<int> hashtagsIDs;
        public bool publicPlaylist;
    }
}
