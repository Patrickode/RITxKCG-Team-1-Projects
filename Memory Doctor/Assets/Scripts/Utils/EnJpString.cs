using UnityEngine;

// Sting class that supports text data in 2 languages

[System.Serializable]
public class EnJpString {
    public string en;
    public string jp;

    public EnJpString (string en, string jp) {
        this.en = en;
        this.jp = jp;
    }

    public string ToStringNewLine () {
        return this.en + "\n" + this.jp;
    }
}