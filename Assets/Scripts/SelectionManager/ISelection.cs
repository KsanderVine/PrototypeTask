namespace DevTask.Selection
{
    public interface ISelection
    {
        void OnSelect(ISelectableRenderer selectable);
        void OnDeselect(ISelectableRenderer selectable);
    }
}