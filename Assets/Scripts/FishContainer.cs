using UnityEngine;

public interface FishContainer
{
    int GetNumberOfFish(); // used to get number of fish in container
    bool IsFull(); // used to determine if container is full
    void AddFish(FishSO fish); // used to add fish to container
    void TakeAllFish(FishContainer newContainer); // used to take all fish from a container to a new container
    void TakeFish(FishContainer newContainer); // used to take a single fish from a container
    FishSO[] GetFish(); // used to get fish in the container
}
