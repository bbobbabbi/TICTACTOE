using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal {
    public Animal(string name)
    {
        Name = name;
    }

    public string Name {  get; set; }
   

    public void Eat() {
        Debug.Log(Name + "is eating");
    }
}

class Dog : Animal
{
    public  Dog(string name) : base(name)
    {
        Name = name;
    }
    public void Bark() {
        Debug.Log(Name + "is barking");
    }
}

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RecursiveFunction(5);
        Dog dog = new Dog("bbobbabbi");
        dog.Bark();
        dog.Eat();
        Animal animal = dog as Animal;
        animal.Eat();
        dog = animal as Dog; 
        dog.Bark();
        dog.Eat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RecursiveFunction(int count) {
        if (count <= 0) {
            Debug.Log("Á¾·á");
            return;
        }
        Debug.Log("Count:" + count);
        RecursiveFunction(count - 1);
    }
}
