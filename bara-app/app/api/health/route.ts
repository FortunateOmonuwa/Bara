import { NextResponse } from 'next/server';

export async function GET() {
  return NextResponse.json(
    { 
      status: 'healthy', 
      timestamp: new Date().toISOString(),
      service: 'bara-frontend'
    },
    { status: 200 }
  );
}
