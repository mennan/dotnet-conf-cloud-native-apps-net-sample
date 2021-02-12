#!/bin/bash
kubectl delete -f ./kubernetes/deployment.yaml
kubectl delete -f ./kubernetes/services.yaml
kubectl delete secret secret-catalog
kubectl delete secret secret-cart
kubectl delete secret secret-ui
docker rmi ecommerceui
docker rmi ecommercecart
docker rmi ecommercecatalog