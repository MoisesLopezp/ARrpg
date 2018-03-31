using System.Collections;
using System.Xml;
using UnityEngine;

public class scr_Lang {

    public static Hashtable UIS;
    public static string language;

    public static void setLanguage()
    {
        language = "English";

        if (scr_Player.Op_Leng == 1)
        {
            language = "Spanish";
        }

        TextAsset textAsset = (TextAsset)Resources.Load("UIStrings");
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textAsset.text);

        UIS = new Hashtable();
        XmlNodeList elements = xml.SelectNodes("/languages/"+ language + "/string");
        //XmlNodeList element = lngs.SelectNodes("/"+language);
        if (elements != null)
        {
            IEnumerator elemEnum = elements.GetEnumerator();
            while (elemEnum.MoveNext())
            {
                XmlElement xmlItem = (XmlElement)elemEnum.Current;
                UIS.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
            }
            
            foreach(scr_UILang lg in GameObject.FindObjectsOfType<scr_UILang>())
            {
                lg.SetMyText();
            }
        }
        else
        {
            Debug.LogError("The specified language does not exist: " + language);
        }
    }

    public static string GetText(string key)
    {
        return UIS[key].ToString();
    }
}
