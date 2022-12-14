using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedUnitsTable = new Dictionary<int, GameObject>();

    public void addSelected(GameObject t_selected)
    {
        int id = t_selected.GetInstanceID();

        if (!(selectedUnitsTable.ContainsKey(id)))
        {
            selectedUnitsTable.Add(id, t_selected);
            t_selected.AddComponent<SelectionComponent>();
            Debug.Log("Added" + id + "to selected dict");
        }
    }

    public void deselect(int id)
    {
        Destroy(selectedUnitsTable[id].GetComponent<SelectionComponent>());
        selectedUnitsTable.Remove(id);
    }


    public void deselectAll()
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedUnitsTable)
        {
            if (pair.Value != null)
            {
                Destroy(selectedUnitsTable[pair.Key].GetComponent<SelectionComponent>());
            }
        }
        selectedUnitsTable.Clear();
    }
}
