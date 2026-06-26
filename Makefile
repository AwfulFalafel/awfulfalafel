APP=awfulfalafel/rails
ECR_REGISTRY=014637180153.dkr.ecr.us-east-1.amazonaws.com
ECR_REPOSITORY=$(ECR_REGISTRY)/$(APP)

build:
	docker build --tag $(APP) --platform=linux/amd64,linux/arm64 .

push: version
	make -C ~ aws-mfa
	make -C ~ aws-ecr-login
	docker tag $(APP) $(ECR_REPOSITORY):latest
	docker tag $(APP) $(ECR_REPOSITORY):`cat version.txt`
	docker push $(ECR_REPOSITORY):latest
	docker push $(ECR_REPOSITORY):`cat version.txt`

version:
	echo `date +%Y%m%d%H%M%S`-`git describe --long --always --dirty` > version.txt