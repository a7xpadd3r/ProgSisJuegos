using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProduct
{
}

public abstract class AbstractFactory<T> where T : IProduct
{
    public abstract T CreateProduct(string productCode);
}
