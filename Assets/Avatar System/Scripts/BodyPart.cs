using UnityEngine;

public class BodyPart
{
    public static string HEAD_NAME = "Head";
    public static string HAIR_NAME = "Hair";
    public static string UPPER_NAME = "Upper";
    public static string AVATAR_NAME = "Avatar";
    public static string LOWER_NAME = "Lower";
    public static string FEET_NAME = "Feet";
    public static string FACIAL_HAIR_NAME = "Facial hair";
    public static string EARRING_NAME = "Earring";
    public static string MASK_NAME = "Mask";
    public static string HAT_NAME = "Hat";
    public static string EYEBROWS_NAME = "Eyebrows";
    public static string EYES_NAME = "Eyes";
    public static string MOUTH_NAME = "Mouth";


    public enum BodyPartType { mesh, texture}
    public string name;
    public string parentName;
    
    public GameObject[] meshes;
    public Texture2D[] thumbnails;
    public Texture[] textures;
    public Texture[] masks;
    public BodyPartType bodyPartType;
    public bool isRemoveable;
}
