using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] private List<Element> elements;
    [SerializeField] private Transform content;
    [SerializeField] private Button elementIcon;
    [SerializeField] private MapPainter painter;
    private bool shown = false;

    private void Start()
    {
        SetElements();
        ToggleContent();
    }

    private void SetElements()
    {
        while(content.childCount > 0) {
            Destroy(content.GetChild(0));
        }

        foreach (Element element in elements)
        {
            Button newIcon = Instantiate(elementIcon, content);
            newIcon.GetComponent<Image>().color = element.Colour;
            newIcon.onClick.AddListener(() => {
                SetElement(element);
            });
        }
    }

    private void SetElement(Element element) {
        painter.SetElement(element);
    }

    public void ToggleContent()
    {
        shown = !shown;
        content.gameObject.SetActive(shown);
    }
}
