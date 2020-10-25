# FunWithCrdInCSharp

Use crd as a storage for a super simple warehouse app.  
A crd for product info with max and min quota.  
A crd for product availability.  
Run the operator project with install parameter to install the crd.  
The operator listen to product info created event to create warehouse product availability with availability equal to product product info max quota.  
The operator listen to product availability update to refill the product availability to the max quota for the product after 1 minute (event is required).  
Use a local version of kubeops c# project (since I needed to fix a few things).  
Kubeops failed to compare crd instances with the local chace due to managedfields that are stored in a jobject).  

Todo:  
expose ui using blazor.  
Protect api using identity server 4, federated with a azure b2c tenant.  

