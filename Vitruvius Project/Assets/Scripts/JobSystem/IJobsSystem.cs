using UnityEngine;
using UnityEngine.AI;

public interface IJobsSystem
{
    void startShift();  // Executed when starting the jobs shift
    void working();     // Job Loop
    void stopShift();   // Executed when finishing the jobs shift
    bool isActive();    // If citizen has arrived to work
    float getMinDistance(); // Returns the minimum distance from teh building to start working
    void setType(string resourceName);  // Sets the type of item we are producing
}
