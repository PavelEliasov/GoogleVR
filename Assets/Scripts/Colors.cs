using UnityEngine;
using System.Collections;

public class Colors {

    static Colors _instance;

    Color[] _color;

    Color[] _tempColorArr=new Color[] {Color.red,Color.green,Color.blue,Color.black };

    public Color this[int i] {
        get {
            
            return _color[i];
        }
       
    }

    public static Colors Instance  {
        get  {
            if (_instance==null) {
                _instance = new Colors(SceneController.Instance.Operations);
            }
            return _instance;
        }

        set{
            _instance = value;
        }
    }

    public Colors(int size) {
        _color = new Color[size];

        for (int i=0; i<_color.Length; ++i) {
            _color[i] = _tempColorArr[i];
        }
      
    }


}
