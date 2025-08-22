# Bara Application - Docker Management
.PHONY: help build up down logs clean dev prod restart health backup restore

# Default target
help:
	@echo "Bara Application Docker Management"
	@echo ""
	@echo "Available commands:"
	@echo "  build      - Build all Docker images"
	@echo "  up         - Start all services (production)"
	@echo "  down       - Stop all services"
	@echo "  logs       - View logs from all services"
	@echo "  clean      - Remove all containers, images, and volumes"
	@echo "  dev        - Start development environment"
	@echo "  prod       - Start production environment"
	@echo "  restart    - Restart all services"
	@echo "  health     - Check health of all services"
	@echo "  backup     - Backup database"
	@echo "  restore    - Restore database from backup"

# Build all images
build:
	@echo "Building Docker images..."
	docker-compose build --no-cache

# Start production environment
up: prod

# Start production environment
prod:
	@echo "Starting production environment..."
	@if [ ! -f .env.production ]; then \
		echo "Error: .env.production file not found!"; \
		echo "Copy .env.production.template to .env.production and configure it."; \
		exit 1; \
	fi
	docker-compose -f docker-compose.yml -f docker-compose.prod.yml --env-file .env.production up -d

# Start development environment
dev:
	@echo "Starting development environment..."
	docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d

# Stop all services
down:
	@echo "Stopping all services..."
	docker-compose down

# View logs
logs:
	docker-compose logs -f

# View logs for specific service
logs-backend:
	docker-compose logs -f backend

logs-frontend:
	docker-compose logs -f frontend

logs-postgres:
	docker-compose logs -f postgres

# Restart all services
restart:
	@echo "Restarting all services..."
	docker-compose restart

# Check health of services
health:
	@echo "Checking service health..."
	@echo "Backend API:"
	@curl -s http://localhost:8080/health | jq . || echo "Backend not responding"
	@echo ""
	@echo "Frontend:"
	@curl -s http://localhost:3000/api/health | jq . || echo "Frontend not responding"
	@echo ""
	@echo "Docker services:"
	@docker-compose ps

# Clean up everything
clean:
	@echo "Cleaning up Docker resources..."
	docker-compose down -v --remove-orphans
	docker system prune -af --volumes

# Backup database
backup:
	@echo "Creating database backup..."
	@mkdir -p backups
	docker-compose exec -T postgres pg_dump -U bara_admin bara > backups/bara_backup_$(shell date +%Y%m%d_%H%M%S).sql
	@echo "Backup created in backups/ directory"

# Restore database (usage: make restore BACKUP_FILE=backups/bara_backup_20240101_120000.sql)
restore:
	@if [ -z "$(BACKUP_FILE)" ]; then \
		echo "Usage: make restore BACKUP_FILE=path/to/backup.sql"; \
		exit 1; \
	fi
	@echo "Restoring database from $(BACKUP_FILE)..."
	docker-compose exec -T postgres psql -U bara_admin bara < $(BACKUP_FILE)

# Update application
update:
	@echo "Updating application..."
	git pull
	docker-compose build --no-cache
	docker-compose up -d

# View resource usage
stats:
	docker stats --format "table {{.Container}}\t{{.CPUPerc}}\t{{.MemUsage}}\t{{.NetIO}}\t{{.BlockIO}}"

# Shell access
shell-backend:
	docker-compose exec backend bash

shell-frontend:
	docker-compose exec frontend sh

shell-postgres:
	docker-compose exec postgres psql -U bara_admin bara

# Quick development setup
dev-setup:
	@echo "Setting up development environment..."
	@if [ ! -f .env.production ]; then \
		cp .env.production.template .env.production; \
		echo "Created .env.production from template. Please configure it."; \
	fi
	make build
	make dev
