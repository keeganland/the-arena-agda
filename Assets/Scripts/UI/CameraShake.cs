using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private float m_magnitude = 0.1f;
    private float shakeIntensity = 0.1f;
    private bool isCoroutineStarted;
    [SerializeField] private float speed = 0.7f; //must be less than 1

    private void Update()
    {
        if (isCoroutineStarted)
        {
            if (shakeIntensity < m_magnitude)
            {
                shakeIntensity += Time.deltaTime * speed/10;
            }
        }
        else
        {
            if(shakeIntensity > 0)
            {
                shakeIntensity -= Time.deltaTime*speed;
            }
        }
    }

    public IEnumerator LaserShake(float duration, float magnitude)
    {
        isCoroutineStarted = true;
        m_magnitude = magnitude;
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float z = Random.Range(-1f, 1f) * shakeIntensity;

            transform.localPosition = new Vector3(x, originalPos.y,z);

            
            elapsed += Time.deltaTime;

            yield return null;
        }

        isCoroutineStarted = false;

        while (shakeIntensity > 0)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float z = Random.Range(-1f, 1f) * shakeIntensity;

            transform.localPosition = new Vector3(x, originalPos.y, z);


            elapsed += Time.deltaTime;

            yield return null;
        }        
        transform.localPosition = originalPos;
    }

}
