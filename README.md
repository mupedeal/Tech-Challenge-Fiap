1 - faça o clone do repositório

2 - navegue até a pasta raiz do projeto

3 - crie os repositórios no dockerhub
mupedeal/contactregister-api-ddd
mupedeal/contactregister-api-readcontact
mupedeal/contactregister-api-writecontact

4 - faça o build das imagens
docker build --no-cache -f src/ContactRegister.Api.Ddd/Dockerfile -t mupedeal/contactregister-api-ddd:latest . && docker build --no-cache -f src/ContactRegister.Api.ReadContact/Dockerfile -t mupedeal/contactregister-api-readcontact:latest . && docker build --no-cache -f src/ContactRegister.Api.WriteContact/Dockerfile -t mupedeal/contactregister-api-writecontact:latest .

5 - publique as imagens no dockerhub
docker push mupedeal/contactregister-api-ddd:latest && docker push mupedeal/contactregister-api-readcontact:latest && docker push mupedeal/contactregister-api-writecontact:latest

6 - crie o banco de dados*
kubectl apply -f sqlserver-secret.yaml && kubectl apply -f sqlserver-configmap.yaml && kubectl apply -f sqlserver-persistent-volume-claim.yaml && kubectl apply -f sqlserver-deployment.yaml && kubectl apply -f sqlserver-service.yaml

* não esqueça de alterar a imagem utilizada nos arquivos *-deployment.yaml

7 - crie o cosmos db
essa aplicação usa o cosmos db; crie e adicione a connection string em CosmosConnection__ConnectionString no arquivo apis-secret.yaml, o nome do database está em apis-configmap.yaml; será criado o container "ddds" dentro do db

8 - crie o rabbitmq
kubectl apply -f rabbitmq-secret.yaml && kubectl apply -f rabbitmq-configmap.yaml && kubectl apply -f rabbitmq-persistent-volume-claim.yaml && kubectl apply -f rabbitmq-deployment.yaml && kubectl apply -f rabbitmq-service.yaml

9 - crie as apis
kubectl apply -f apis-secret.yaml && kubectl apply -f apis-configmap.yaml && kubectl apply -f apis-deployment-ddd.yaml && kubectl apply -f apis-deployment-read-contact.yaml && kubectl apply -f apis-deployment-write-contact.yaml && kubectl apply -f apis-service-ddd.yaml && kubectl apply -f apis-service-read-contact.yaml && kubectl apply -f apis-service-write-contact.yaml

10 - verifique
kubectl get secret,cm,pv,pvc,deployment,pods,svc

11 - crie o prometheus
kubectl apply -f prometheus-configmap.yaml && kubectl apply -f prometheus-persistent-volume-claim.yaml && kubectl apply -f prometheus-deployment.yaml && kubectl apply -f prometheus-service.yaml

12 - crie o grafana
kubectl apply -f grafana-secret.yaml && kubectl apply -f grafana-persistent-volume-claim.yaml && kubectl apply -f grafana-deployment.yaml && kubectl apply -f grafana-service.yaml

13 - importar o dashboard do grafana
importar o arquivo contact-register-grafana-dashboard.json para o grafana via interface web

* ajustar o source do dashboard
