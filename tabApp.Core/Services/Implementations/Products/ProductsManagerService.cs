﻿using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations.Products
{
    public class ProductsManagerService : IProductsManagerService
    {
        private List<Product> _productsList;

        public List<Product> ProductsList => _productsList;

        public double GetProductAmmount(int clientId, Product product)
        {
            foreach(var item in product.ReSaleValues ?? new List<(int Id, double Value)>())
            {
                if (clientId == item.Id)
                    return item.Value;
            }

            return product.PVP;
        }

        public Product GetProductByClosestName(string productName)
        {
            Product produto = null;
            double sim = 0;
            double simProv;

            if (productName.Equals("Bolos pastelaria") || productName.Length == 0)
                return null;

            foreach (Product item in ProductsList)
            {
                simProv = Compare(productName, item.Name);
                if (simProv > sim)
                {
                    sim = simProv;
                    produto = item;
                }
            }
            return produto;
        }

        private double Compare(string name, string compareName)
        {
            int length = name.Length;
            int comLength = compareName.Length;
            int max = length;
            int min = comLength;
            int result = 0;
            if (length < comLength)
            {
                max = comLength;
                min = length;
            }

            for (int index = 0; index < min; index++)
            {
                if (name[index] == compareName[index])
                {
                    result++;
                }
            }
            return (double)(result) / (double)(max);
        }

        public Product GetProductById(int productId)
        {
            return _productsList.Find(item => item.Id == productId);
        }

        public string GetProductNameById(int productId)
        {
            return _productsList.Find(item => item.Id == productId).Name;
        }

        public void SetProducts(List<Product> productsList)
        {
            _productsList = productsList;
        }
    }
}
