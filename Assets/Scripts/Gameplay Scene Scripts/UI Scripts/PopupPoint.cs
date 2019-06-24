using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPoint : MonoBehaviour
{
    [SerializeField]
    private Text pointsText;

    [SerializeField]
    private float destroyTime = 1.5f;

    [SerializeField]
    private float moveSpeed = 0.1f;

    private RectTransform rectTransform;

    [SerializeField]
    private Color negativeColor = Color.red;



    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        float yRotation = Mathf.Clamp(Camera.main.transform.position.y, -45, 45);

        //pointsText.gameObject.GetComponent<RectTransform>().eulerAngles = new Vector3(Camera.main.transform.position.x, yRotation, Camera.main.transform.position.z);
        pointsText.gameObject.GetComponent<RectTransform>().rotation = Camera.main.transform.rotation; //Quaternion.Euler(Camera.main.transform.rotation, 0, 0);

        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        rectTransform.localPosition += rectTransform.up * moveSpeed * Time.deltaTime;
    }

    public void SetText(string text)
    {
        pointsText.text = text;

        if (text.StartsWith("-"))
        {
            pointsText.color = negativeColor;
        }
    }
}
