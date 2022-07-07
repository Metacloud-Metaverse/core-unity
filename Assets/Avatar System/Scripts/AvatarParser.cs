using UnityEngine;

public class AvatarParser
{
    public AvatarSystem cc;

    public void ApplyConfigToMesh(string json, GameObject avatar)
    {
        var avatarInfo = JsonUtility.FromJson<AvatarInfo>(json);

        ApplyConfigToMesh(avatarInfo, avatar);
    }

    public void ApplyConfigToMesh(AvatarInfo avatarInfo, GameObject avatar, Transform parent = null)
    {

        var sex = (avatarInfo.sex == 0) ? true : false;
        var newAvatar = AvatarSystem.Instance.SetBaseMesh(sex, avatar, parent);

        AvatarSystem.Instance.SetSkinColor(avatarInfo.skin.index, newAvatar);

        SetBodyPart(AvatarSystem.HAIR, avatarInfo.hair, newAvatar);
        SetBodyPart(AvatarSystem.UPPER, avatarInfo.upper, newAvatar);
        SetBodyPart(AvatarSystem.LOWER, avatarInfo.lower, newAvatar);
        SetBodyPart(AvatarSystem.FEET, avatarInfo.feet, newAvatar);

        SetBodyPart(AvatarSystem.MASK, avatarInfo.mask, newAvatar);
        SetBodyPart(AvatarSystem.HAT, avatarInfo.hat, newAvatar);
        SetBodyPart(AvatarSystem.EARRING, avatarInfo.earring, newAvatar);

        SetBodyPart(AvatarSystem.EYES, avatarInfo.eyes, newAvatar);
        SetBodyPart(AvatarSystem.MOUTH, avatarInfo.mouth, newAvatar);
        SetBodyPart(AvatarSystem.EYEBROWS, avatarInfo.eyebrows, newAvatar);
        SetBodyPart(AvatarSystem.FACIAL_HAIR, avatarInfo.facialHair, newAvatar);

        AvatarSystem.Instance.SetEyesColor(avatarInfo.eyesColor.index, newAvatar);
        AvatarSystem.Instance.SetHeadHairColor(avatarInfo.hairColor.index, newAvatar);
        AvatarSystem.Instance.SetEyebrowsColor(avatarInfo.eyebrowsColor.index, newAvatar);
        AvatarSystem.Instance.SetFacialHairColor(avatarInfo.facialHairColor.index, newAvatar);
    }

    private void SetBodyPart(int bodyPartIndex, WereableInfo info, GameObject avatar)
    {
        if (info == null) return;

        if (info.type == "default")
            AvatarSystem.Instance.SetBodyPart(bodyPartIndex, info.index, avatar);

        else if (info.type == "custom")
            AvatarSystem.Instance.SetBodyPart(bodyPartIndex, info.url, avatar);
    }
}

[System.Serializable]
public class WereableInfo
{
    public int index;
    public string type;
    public string thumbnail;
    public string url;
}

[System.Serializable]
public class ColorInfo
{
    public int index;
}

[System.Serializable]
public class AvatarInfo
{
    public int sex;
    public WereableInfo hair;
    public WereableInfo upper;
    public WereableInfo lower;
    public WereableInfo feet;
    public WereableInfo mask;
    public WereableInfo hat;
    public WereableInfo earring;
    public WereableInfo eyes;
    public WereableInfo mouth;
    public WereableInfo eyebrows;
    public WereableInfo facialHair;
    public ColorInfo skin;
    public ColorInfo eyesColor;
    public ColorInfo hairColor;
    public ColorInfo facialHairColor;
    public ColorInfo eyebrowsColor;

}
