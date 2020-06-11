using UnityEngine;


public class MoleculeIdenti : MonoBehaviour
{
    public SimulationService simulationService;
    public bool SelectedMolecule;
    public MoleculesName Name;


    public void ChangeToSlectedColor()
    {
        simulationService.UnselectAllMolecule();
        GetComponent<MeshRenderer>().material = simulationService.SelectedMat;
        SelectedMolecule = true;
    }


    public void ChangeToNormalColor()
    {
        GetComponent<MeshRenderer>().material = simulationService.NormalMat;
        SelectedMolecule = false;
    }
}
