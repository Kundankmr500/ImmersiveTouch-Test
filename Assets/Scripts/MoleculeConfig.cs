using UnityEngine;

[CreateAssetMenu(fileName = "MoleculeConfig", menuName = "ScriptableObjects/NewMoleculeInfo")]
public class MoleculeConfig : ScriptableObject
{
    public GameObject MoleculePrefab;
    public MoleculesName MoleculeName;
}


public enum MoleculesName
{
    Carbon_Dyoxide,
    Mithane,
    Water_Molecule
}


public class MoleculeInfo
{
    public Vector3 Position;
    public Vector3 Rotation;
    public MoleculesName MoleculeName;
}
