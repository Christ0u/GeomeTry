using UnityEngine;

public class CharacterPortal : MonoBehaviour
{
    public enum PortalType
    {
        CubePortal,
        ShipPortal,
        WavePortal
    }
    
    [SerializeField] private PortalType portalType;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private GameObject wavePrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        
        if (character != null) // safety check, probablement inutile
        {
            TransformPlayer(character);
        }
    }

    private void TransformPlayer(Character character)
    {
        GameObject newCharacterPrefab = null;
        Camera mainCamera = FindFirstObjectByType<Camera>();
        
        switch (portalType)
        {
            case PortalType.CubePortal:
                newCharacterPrefab = cubePrefab;
                break;
            case PortalType.ShipPortal:
                newCharacterPrefab = shipPrefab;
                break;
            case PortalType.WavePortal:
                newCharacterPrefab = wavePrefab;
                break;
        }

        if (newCharacterPrefab != null)
        {
            Vector3 position = character.transform.position;
            Quaternion rotation = character.transform.rotation;

            // Merci Christ0u pour le tips 
            Destroy(character.gameObject);
            
            GameObject newCharacter = Instantiate(newCharacterPrefab, position, rotation);

            // On réattache la caméra sinon elle ne suit plus le joueur (Detruit avant) 
            if (mainCamera != null)
            {
                mainCamera.player = newCharacter.transform;
            }
        }
    }
}