FROM grafana/grafana:12.1

ENV PROMETHEUS_HOST='host.docker.internal:9090'

COPY dashboards /monitoring/grafana/dashboards
COPY datasources /monitoring/grafana/datasources

EXPOSE 3000