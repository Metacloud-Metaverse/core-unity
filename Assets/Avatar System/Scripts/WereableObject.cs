using UnityEngine;
using UnityEngine.UI;

public class WereableObject
{
    public enum Sex { male, female, unisex }

    public Sex sex;
    public Object part;
    public int index;
    public Texture thumbnail;

    public WereableObject(Sex sex, Object part, int index)
    {
        this.sex = sex;
        this.part = part;
        this.index = index;
        
    }
}
