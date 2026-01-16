using System;
using System.Collections.Generic;

/// <summary>
/// This queue is circular. When people are added via AddPerson, they are added to the 
/// back of the queue (FIFO). When GetNextPerson is called, the next person
/// is dequeued and returned. If they have turns left, they are re-enqueued.
/// Turns = 0 or less → infinite turns.
/// </summary>
public class TakingTurnsQueue
{
    // Use a proper FIFO queue
    private readonly Queue<Person> _people = new Queue<Person>();

    // Current number of people in the queue
    public int Length => _people.Count;

    /// <summary>
    /// Add a new person to the queue with a name and number of turns
    /// </summary>
    public void AddPerson(string name, int turns)
    {
        var person = new Person(name, turns);
        _people.Enqueue(person);
    }

    /// <summary>
    /// Get the next person in the queue and return them.
    /// Re-enqueue if they have turns left or infinite turns.
    /// </summary>
    public Person GetNextPerson()
    {
        if (_people.Count == 0)
            throw new InvalidOperationException("No one in the queue.");

        // Remove the person at the front
        Person person = _people.Dequeue();

        // Decide if they should be re-enqueued
        if (person.Turns > 1)
        {
            // Finite turns > 1 → decrement and re-enqueue
            person.Turns -= 1;
            _people.Enqueue(person);
        }
        else if (person.Turns <= 0)
        {
            // Infinite turns → re-enqueue without decrement
            _people.Enqueue(person);
        }
        // else Turns == 1 → last turn, do not re-enqueue

        return person;
    }

    public override string ToString()
    {
        return string.Join(", ", _people);
    }
}
