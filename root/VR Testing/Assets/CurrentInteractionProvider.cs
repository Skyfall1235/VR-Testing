/* Because i will not remember these specific references for the select and 
 * hovering of interactors or interactables, this script should provide an 
 * easy to refence sheet */
using UnityEngine.XR.Interaction.Toolkit;

public class CurrentInteractionProvider
{
    #region Interactors
    /// <summary>
    /// Retrieves a specific hover interactor that's currently hovering over a given interactable object.
    /// </summary>
    /// <param name="interactable"> The interactable object to check for hovering interactors.</param>
    /// <param name="index"> The index of the desired hover interactor within the interactable's list of hovering interactors.</param>
    /// <returns> An IXRHoverInteractor object representing the hover interactor at the specified index, or potentially null if no interactor is found at that index.</returns>
    public static IXRHoverInteractor FindHoverInteractorInIndex(XRBaseInteractable interactable, int index)
    {
        IXRHoverInteractor interactor = interactable.interactorsHovering[index];
        return interactor;
    }

    /// <summary>
    /// Retrieves a specific select interactor that's currently selecting a given interactable object.
    /// </summary>
    /// <param name="interactable"> The interactable object to check for selecting interactors.</param>
    /// <param name="index"> The index of the desired select interactor within the interactable's list of select interactors.</param>
    /// <returns> An IXRSelectInteractor object representing the select interactor at the specified index, or potentially null if no interactor is found at that index.</returns>
    public static IXRSelectInteractor FindSelectInteractorInIndex(XRBaseInteractable interactable, int index)
    {
        IXRSelectInteractor interactor = interactable.interactorsSelecting[index];
        return interactor;
    }
    #endregion

    #region Interactables
    /// <summary>
    /// Retrieves a specific interactable object that's currently being hovered over by a given hover interactor.
    /// </summary>
    /// <param name="interactor"> The hover interactor to check for hovered interactables.</param>
    /// <param name="index"> The index of the desired hovered interactable within the interactor's list of hovered interactables.</param>
    /// <returns>An IXRHoverInteractable object representing the interactable being hovered over at the specified index, or potentially null if no interactable is found at that index.</returns>
    public static IXRHoverInteractable FindHoverInteractableInIndex(XRBaseInteractor interactor, int index)
    {
        IXRHoverInteractable interactable = interactor.interactablesHovered[index];
        return interactable;
    }

    /// <summary>
    /// Retrieves a specific select interactor that's currently selecting a given interactable object.
    /// </summary>
    /// <param name="interactor"> The Select interactor to check for selected interactables.</param>
    /// <param name="index"> The index of the desired selected interactable within the interactor's list of selected interactables.</param>
    /// <returns>An IXRHoverInteractable object representing the interactable being selected at the specified index, or potentially null if no interactable is found at that index.</returns>
    public static IXRSelectInteractable FindSelectInteractableInIndex(XRBaseInteractor interactor, int index)
    {
        IXRSelectInteractable interactable = interactor.interactablesSelected[index];
        return interactable;
    }
    #endregion
}
