# build stage
FROM node:18-alpine AS build
WORKDIR /app

ARG API_BASE_URL="http://localhost:5191/api"
ENV API_BASE_URL=${API_BASE_URL}

COPY package*.json ./
RUN npm ci

COPY . .

# replace the API_BASE_URL constant in source before building
# (only tries if file exists)
RUN if [ -f src/App.jsx ]; then \
      sed -i "s|const API_BASE_URL = .*;|const API_BASE_URL = '${API_BASE_URL}';|" src/App.jsx || true; \
    fi

RUN npm run build

# production stage: nginx
FROM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]