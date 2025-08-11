FROM node:24-alpine AS build

ARG PORT=9093
RUN addgroup --system awfulfalafel && adduser --system awfulfalafel awfulfalafel
USER awfulfalafel:awfulfalafel
WORKDIR /build
ADD package*.json tsconfig.json .env ./
ADD app ./app
RUN npm ci
RUN npm run build

EXPOSE $PORT
CMD [ "npm", "run", "serve" ]

FROM build AS deploy

ARG PORT=9093

WORKDIR /app
ADD package*.json ./
COPY --from=build /build/dist ./
RUN npm ci --omit=dev && npm cache clean --force

EXPOSE $PORT
CMD [ "node", "index.js" ]
