using UnityEngine;

public interface IIEffect
{
    void Effect();
}

[CreateAssetMenu(fileName = "BaseUpgrade", menuName = "Scriptable Objects/BaseUpgrade")]
public abstract class BaseUpgrade : ScriptableObject, IIEffect
{
    [Header("Title of Upgrade")]
    public string Title;

    [Header("Description of Upgrade")]
    [TextArea(3, 10)]
    public string Description;

    [Header("Image of Upgrade")]
    public Sprite image;
    public abstract void Effect();
}






















