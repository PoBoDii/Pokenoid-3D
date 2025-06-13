using UnityEngine;

public class controlLadrillo : MonoBehaviour
{
    public GameObject prefabCapsula;
    public bool sueltaCapsula = false;

    public GameObject explosionPrefab;

    public void DestruyeLadrillo()
    {
        if (sueltaCapsula && prefabCapsula != null)
        {
            Instantiate(prefabCapsula, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.Euler(0f, 0f, 0f));
        }

        MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

        if (childRenderers.Length > 0)
        {
            // Si tiene hijos con MeshRenderer, genera partículas en cada uno
            foreach (var renderer in childRenderers)
            {
                GameObject psObj = Instantiate(explosionPrefab, renderer.transform.position, Quaternion.identity);
                ParticleSystem ps = psObj.GetComponent<ParticleSystem>();
                ParticleSystemRenderer psr = psObj.GetComponent<ParticleSystemRenderer>();

                if (renderer.material != null)
                {
                    psr.material = renderer.material;
                }

                Destroy(psObj, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }
        else
        {
            // Si no tiene hijos, genera uno solo para el objeto principal
            GameObject psObj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = psObj.GetComponent<ParticleSystem>();
            ParticleSystemRenderer psr = psObj.GetComponent<ParticleSystemRenderer>();

            // Intenta buscar material propio
            Material mat = null;
            MeshRenderer selfRenderer = GetComponent<MeshRenderer>();
            if (selfRenderer != null)
            {
                mat = selfRenderer.material;
            }
            if (mat != null)
            {
                psr.material = mat;
            }

            Destroy(psObj, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        // Notificamos al gestor y reproducimos sonido
        GameObject GestorLadrillos = GameObject.FindGameObjectWithTag("GestorLadrillos");
        LadrilloManager script = GestorLadrillos.GetComponent<LadrilloManager>();
        script.LadrilloDestruido();

        AudioManager.instance.BreakBlockSound();
        Destroy(gameObject, 0.1f);
    }
}
