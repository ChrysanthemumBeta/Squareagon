using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Inventory inv;
    public DisplayInventory DispInv;
    public bool Drag;
    private GameObject Icon;
    public RectTransform Plane;
    InventoryItem Self;
    int PrevSlotNumber;

    public void OnBeginDrag(PointerEventData eventData)
    {
        PrevSlotNumber = GetSlotNumber(gameObject);
        if (inv.InventoryIsOpen)
        {
            Self = inv.DeleteFrom(PrevSlotNumber);
        }
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
        { 
            return;
        }

        if(Self.count > 0 && inv.InventoryIsOpen)
        {
            Icon = new GameObject("icon");

            Icon.transform.SetParent(canvas.transform, false);
            Icon.transform.SetAsLastSibling();

            var image = Icon.AddComponent<Image>();

            image.sprite = GetComponent<Image>().sprite;
            image.SetNativeSize();

            if (Drag)
            {
                Plane = transform as RectTransform;
            }
            else
            {
                Plane = canvas.transform as RectTransform;
            }
            SetDraggedPosition(eventData);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Icon != null)
        {
            SetDraggedPosition(eventData);
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (Icon != null && inv.InventoryIsOpen)
        {
            bool IsInInv = false;
            foreach (Image item in DispInv.Slots)
            {
                if(RectTransformUtility.RectangleContainsScreenPoint(item.gameObject.GetComponent<RectTransform>(), Icon.transform.position))
                {
                    Debug.Log("Yes");
                    int NewSlot = GetSlotNumber(item.gameObject);
                    Debug.Log(NewSlot);
                    InventoryItem New = inv.GetInvItem(NewSlot);
                    bool Swap = false;
                    if(New.IsItem == Self.IsItem)
                    {
                        if (New.IsItem)
                        {
                            if(New.ItemData == Self.ItemData)
                            {

                            }
                        }
                        else
                        {
                            if(New.Data == Self.Data)
                            {
                                if(New.count + Self.count <= Self.Data.StackLimit)
                                {
                                    Self.count += New.count;
                                    New.count = 0;
                                }
                                else
                                {
                                    int amount = (New.count + Self.count) - Self.Data.StackLimit;
                                    Self.count = Self.Data.StackLimit;
                                    New.count = amount;
                                }
                            }
                        }
                    }
                    inv.SetItem(NewSlot, Self);
                    if (New.count > 0)
                    {
                        inv.SetItem(PrevSlotNumber, New);
                    }
                    IsInInv = true;
                    break;
                }
            }
            if (!IsInInv)
            {
                Debug.Log("No");
                inv.SetItem(PrevSlotNumber, Self);
                Debug.Log(PrevSlotNumber);
                Debug.Log(Self.count);
                inv.InventoryData[PrevSlotNumber] = Self;
            }
            Destroy(Icon);
        }
    }
    private void SetDraggedPosition(PointerEventData data)
    {
        if (Drag && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            Plane = data.pointerEnter.transform as RectTransform;

        var rt = Icon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(Plane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = Plane.rotation;
        }
    }
    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    int GetSlotNumber(GameObject Check)
    {
        int i = 0;
        foreach (Image item in DispInv.Slots)
        {
            if (item == Check.GetComponent<Image>())
            {
                return i;
            }
            i++;
        }
        return -1;
    }
}
