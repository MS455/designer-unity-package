using TMPro;
using UnityEngine;
using System.Collections;
using UnityAtoms.BaseAtoms;
using System.Collections.Generic;

public class MRSavePanelItem : MonoBehaviour
{
    [SerializeField]
    int placement_id;

    [SerializeField]
    ARStateVIew parentView;

    [SerializeField]
    TextMeshProUGUI tmp_text;

    public void SetInfo( int a_placement_id , ARStateVIew a_parentView)
    {
        this.placement_id = a_placement_id;
        this.parentView = a_parentView;

        tmp_text.text = "Experience " + placement_id.ToString();
    }

    public void DeleteThis()
    {
        //parentView.DeleteItem(id , placement_id);
        //parentView.OpenDeleteConfiramationPanel(this, placement_id, tmp_text.text);
    }

    public void RestoreThis()
    {
        //parentView.RestoreItem( placement_id);
    }
}