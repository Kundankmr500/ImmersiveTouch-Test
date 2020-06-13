using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SimulationService : MonoBehaviour {

    
    public List<MoleculeConfig> MoleculesConfig = new List<MoleculeConfig>();
    public Transform SceneObjectParent;
    public Transform UINameParent;
    public TextMeshProUGUI MoleculeNameUIPrefab;
    public Dropdown Dropdown;
    public Material SelectedMat;
    public Material NormalMat;

    [SerializeField]
    private List<GameObject> AllMoleculeInScene = new List<GameObject>();
    [SerializeField]
    private List<TextMeshProUGUI> MoleculeNameUIList = new List<TextMeshProUGUI>();
    private List<MoleculeInfo> moleculeInfos = new List<MoleculeInfo>();
    List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();

    private const string fileName = "/scene_json.txt";



    private void Start()
    {
        AddDropDownOption();
    }


    private void AddDropDownOption()
    {
        for (int i = 0; i < MoleculesConfig.Count; i++)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = MoleculesConfig[i].MoleculeName.ToString();
            optionDatas.Add(optionData);
        }
        Dropdown.AddOptions(optionDatas);
    }


    public void AddMoleculesFromDropdown()
    {
        InstantiateMolecule((MoleculesName)Dropdown.value, GetRandomPos(), new Vector3(0, 0, 0));
    }


    public void SaveData()
    {
        moleculeInfos.Clear();
        for (int i = 0; i < AllMoleculeInScene.Count; i++)
        {
            MoleculeInfo moleculeInfo = new MoleculeInfo();
            moleculeInfo.Position = AllMoleculeInScene[i].transform.position;
            moleculeInfo.Rotation = AllMoleculeInScene[i].GetComponent<ObjectDrag>().Atom.transform.eulerAngles;
            moleculeInfo.MoleculeName = AllMoleculeInScene[i].GetComponent<MoleculeIdenti>().Name;

            moleculeInfos.Add(moleculeInfo);
        }

        string path = Application.streamingAssetsPath + fileName;
        string jsonString = JsonConvert.SerializeObject(moleculeInfos, Formatting.Indented);

        File.WriteAllText(path, jsonString);

        Debug.Log("Scene Saved ");
    }


    private GameObject InstantiateMolecule(MoleculesName name , Vector3 position, Vector3 rotation)
    {
        GameObject molecule = Instantiate(MoleculesConfig[(int)name].MoleculePrefab, SceneObjectParent);
        molecule.GetComponent<MoleculeIdenti>().simulationService = this;
        molecule.GetComponent<MoleculeIdenti>().Name = name;
        molecule.transform.position = position;
        molecule.GetComponent<ObjectDrag>().Atom.transform.eulerAngles = rotation;
        molecule.name = name.ToString();
        AllMoleculeInScene.Add(molecule);
        AddToSceneUIList(molecule.name);
        return molecule;
    }


    private void AddToSceneUIList(string moleculeName)
    {
        TextMeshProUGUI moleculeNameUI = Instantiate(MoleculeNameUIPrefab, UINameParent);
        moleculeNameUI.text = moleculeName;
        MoleculeNameUIList.Add(moleculeNameUI);
    }


    public void UnselectAllMolecule()
    {
        for (int i = 0; i < AllMoleculeInScene.Count; i++)
        {
            AllMoleculeInScene[i].GetComponent<MoleculeIdenti>().ChangeToNormalColor();
        }
    }


    public void DeleteSelectedMolecule()
    {
        for (int i = 0; i < AllMoleculeInScene.Count; i++)
        {
            if(AllMoleculeInScene[i].GetComponent<MoleculeIdenti>().SelectedMolecule)
            {
                print(AllMoleculeInScene[i] + " Deleted");
                Destroy(MoleculeNameUIList[i].gameObject, 0);
                Destroy(AllMoleculeInScene[i], 0);
                AllMoleculeInScene.RemoveAt(i);
                MoleculeNameUIList.RemoveAt(i);
            }
        }
    }


    public void AddRandomMolecule()
    {
        int randomIndex = Random.Range(0, MoleculesConfig.Count);
        InstantiateMolecule((MoleculesName)randomIndex, GetRandomPos() ,new Vector3(0, 0, 0));
        
        Debug.Log(AllMoleculeInScene.Count);
    }


    Vector3 GetRandomPos()
    {
        Vector3 randomPos = new Vector3(Random.Range(-15, 20), Random.Range(-1, 15), 0);
        return randomPos;
    }


    public void ClearScene()
    {
        for (int i = 0; i < AllMoleculeInScene.Count; i++)
        {
            Destroy(AllMoleculeInScene[i]);
            Destroy(MoleculeNameUIList[i].gameObject);
        }
        AllMoleculeInScene.Clear();
        MoleculeNameUIList.Clear();
        moleculeInfos.Clear();
    }


    private void LoadMolecule(List<MoleculeInfo> moleculeInfos)
    {
        for (int i = 0; i < moleculeInfos.Count; i++)
        {
            InstantiateMolecule(moleculeInfos[i].MoleculeName, moleculeInfos[i].Position, moleculeInfos[i].Rotation);
        }
    }


    public void LoadData()
    {
        string path = Application.streamingAssetsPath + fileName;

        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            if (jsonString.Length > 0)
            {
                moleculeInfos = JsonConvert.DeserializeObject<List<MoleculeInfo>>(jsonString);
                LoadMolecule(moleculeInfos);
                Debug.Log(moleculeInfos.Count);
                Debug.Log("Scene loaded ");
            }
            else
            {
                Debug.Log("File is blank");
            }
        }
        else
        {
            Debug.Log("File not found in " + path);
        }
    }
}
