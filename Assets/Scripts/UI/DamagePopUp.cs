using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private TMP_Text textMesh;

    private void Awake()
    {
        textMesh = transform.GetComponent<TMP_Text>();
    }

    public static DamagePopUp CreatePopUp(GameObject gameObject, Transform transform, int Damage)
    {
        gameObject = Instantiate(gameObject, transform.position, Quaternion.identity);

        DamagePopUp damagePopUp = gameObject.GetComponent<DamagePopUp>();

        if (damagePopUp == null)
        {
            Debug.Log("This object does not contain a damagepopup componet");
            return null;
        }

        else
        {
            damagePopUp.Setup(Damage);
            return damagePopUp;
        }

    }

    public static DamagePopUp CreatePopUp(GameObject gameObject, Transform transform, string Damage)
    {
        gameObject = Instantiate(gameObject, transform.position, Quaternion.identity);

        DamagePopUp damagePopUp = gameObject.GetComponent<DamagePopUp>();

        if (damagePopUp == null)
        {
            Debug.Log("This object does not contain a damagepopup componet");
            return null;
        }

        else
        {
            damagePopUp.Setup(Damage);
            return damagePopUp;
        }

    }
    public void Setup(int Damage)
    {
        textMesh.text = Damage.ToString();
        StartCoroutine(TransformText(textMesh));
    }

    public void Setup(string Damage)
    {
        textMesh.text = Damage;
        StartCoroutine(TransformText(textMesh));
    }

    private IEnumerator TransformText(TMP_Text text, float Duration = 0.5f)
    {
        float moveYSpeed = 3f;
        float currentTime = 0f;

        Color TextmeshColor = textMesh.color;

        while (currentTime < Duration)
        {
            transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
            currentTime += Time.deltaTime;

            if (currentTime >= Duration / 2)
            {
                float fadeProgress = (currentTime - (Duration / 2f)) / (Duration / 2f);
                TextmeshColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
                textMesh.color = TextmeshColor;
            }

            yield return null;
        }

        Destroy(gameObject);

    }
}
