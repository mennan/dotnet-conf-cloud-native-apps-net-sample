#!/bin/bash
docker build -t ecommercecatalog -f Dockerfile.catalog .
docker build -t ecommercecart -f Dockerfile.cart .
docker build -t ecommerceui -f Dockerfile.ui .