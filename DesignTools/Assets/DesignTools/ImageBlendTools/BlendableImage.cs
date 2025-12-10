using UnityEngine;
using UnityEngine.UI;

public class BlendableImage : Image
{
    public override Material material
    {
        get
        {
            if (base.m_Material == null)
            {
                Shader blendShader = Shader.Find("UI/Blendable");
                
                if (blendShader == null)
                    return defaultETC1GraphicMaterial;
                else
                    return new Material(blendShader);
            }
            else
            {
                return base.material;
            }
        }

        set
        {
            base.material = value;
        }
    }
}
