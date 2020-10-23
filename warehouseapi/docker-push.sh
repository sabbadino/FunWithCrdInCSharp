#docker login -u sabbadino
#get timestamp for the tag  
timestamp=$(date +%Y%m%d%H%M%S)  
  
tagTimeStamp=$timestamp  
tagExplicit=1.2

docker tag clusterpodcallk8sapi:latest sabbadino/clusterpodcallk8sapi:latest
docker push sabbadino/clusterpodcallk8sapi:latest

docker tag clusterpodcallk8sapi:latest sabbadino/clusterpodcallk8sapi:$tagTimeStamp
docker push sabbadino/clusterpodcallk8sapi:$tagTimeStamp

docker tag clusterpodcallk8sapi:latest sabbadino/clusterpodcallk8sapi:$tagExplicit
docker push sabbadino/clusterpodcallk8sapi:$tagExplicit
