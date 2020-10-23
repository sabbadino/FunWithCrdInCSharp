kind.exe delete cluster --name warehouse
sleep 10
kind.exe create cluster --config kind-create-cluster-settings.yaml --name warehouse
#install dashboard
kubectl.exe apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.0.4/aio/deploy/recommended.yaml
#give dashboard service account cluster admin rights
kubectl.exe apply -f assigndashboardusertocluster-admin.yaml

# install ngnix ingress controller
kubectl.exe apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/master/deploy/static/provider/kind/deploy.yaml
kubectl.exe wait --namespace ingress-nginx   --for=condition=ready pod   --selector=app.kubernetes.io/component=controller   --timeout=90s



# get dashboard secret
kubectl.exe -n kubernetes-dashboard describe secret $(kubectl -n kubernetes-dashboard get secret | grep kubernetes-dashboard | awk '{print $1}')
#kubectl proxy
#http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/


eyJhbGciOiJSUzI1NiIsImtpZCI6IlptRXBsZEFOR2VnTTRJckUwLVM0Zkw1VGpkU1VuWEVzNUwyazljVXdmWXcifQ.eyJpc3MiOiJrdWJlcm5ldGVzL3NlcnZpY2VhY2NvdW50Iiwia3ViZXJuZXRlcy5pby9zZXJ2aWNlYWNjb3VudC9uYW1lc3BhY2UiOiJrdWJlcm5ldGVzLWRhc2hib2FyZCIsImt1YmVybmV0ZXMuaW8vc2VydmljZWFjY291bnQvc2VjcmV0Lm5hbWUiOiJrdWJlcm5ldGVzLWRhc2hib2FyZC10b2tlbi13ZHRwcCIsImt1YmVybmV0ZXMuaW8vc2VydmljZWFjY291bnQvc2VydmljZS1hY2NvdW50Lm5hbWUiOiJrdWJlcm5ldGVzLWRhc2hib2FyZCIsImt1YmVybmV0ZXMuaW8vc2VydmljZWFjY291bnQvc2VydmljZS1hY2NvdW50LnVpZCI6ImVlMjAwMjAwLWIxMzctNDhlYi1hZmEyLWNlODdjNTExYTJjYyIsInN1YiI6InN5c3RlbTpzZXJ2aWNlYWNjb3VudDprdWJlcm5ldGVzLWRhc2hib2FyZDprdWJlcm5ldGVzLWRhc2hib2FyZCJ9.FE9QxGYJCaG5QkrmiHSMjTMYhXq2GCRI64sW2gPab1x1cQIn5nGIcJf3VBONyVNZ6DXlWTJVtsVPlv9yIi2m_7BrRIMRH8xWEZ-F-kv3oduPcGIuICqYN0ldzLZD6xe83z3R_91vz402olHBDeH1imxUIzcGYFwPZwiJHX4mKfhK8NSuO0zfA5fXJMlV8zRNijMeupxNu2Mw9_AOzZBxxka-tG-rP-sHyqDPbTwOyXU3RTTh7mF5BjmKQxrGxKblV92hjbyvruZggUKWxpklenF9efj0vNdbTRLaBsdNgoSJWuDRnxukXbK0toOzmu0H0b6jHkIeJRjR3-mhwKo09Q