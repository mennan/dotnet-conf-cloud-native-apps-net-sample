#!/bin/bash
kubectl create secret generic secret-catalog --from-file=./settings/catalog.settings.json
kubectl create secret generic secret-cart --from-file=./settings/cart.settings.json
kubectl create secret generic secret-ui --from-file=./settings/ui.settings.json
kubectl apply -f ./kubernetes/services.yaml
kubectl apply -f ./kubernetes/deployment.yaml