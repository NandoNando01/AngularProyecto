import { readFileSync, existsSync } from 'fs';
import { join, dirname } from 'path';
import { fileURLToPath } from 'url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const specPath = join(__dirname, '..', 'specs', 'openapi.yaml');

if (!existsSync(specPath)) {
  console.error('ERROR: openapi.yaml not found at', specPath);
  process.exit(1);
}

const content = readFileSync(specPath, 'utf-8');

if (!content.includes('openapi:') || !content.includes('paths:') || !content.includes('components:')) {
  console.error('ERROR: openapi.yaml is missing required sections');
  process.exit(1);
}

const endpoints = content.match(/^\s{2}\/\w+:/gm);
if (!endpoints || endpoints.length === 0) {
  console.error('ERROR: No endpoints found in spec');
  process.exit(1);
}

console.log(`✓ openapi.yaml is valid (${endpoints.length} endpoints found)`);