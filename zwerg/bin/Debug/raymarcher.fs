// DistanceFieldEditor, raymarcher, May 2014, movAX13h, filip.sound@gmail.com

uniform vec2 resolution;
uniform float time;
//uniform vec4 mouse;

uniform int showGrid;
uniform int showAxis;
uniform int showSun;

uniform vec3 camPos;
uniform vec3 camTarget;

uniform vec3 lightPos;
uniform vec3 lightPosNormalized;

uniform float diffuseIntensity;
uniform vec3 diffuseColor;

uniform float specularIntensity;
uniform vec3 specularColor;
uniform float specularFallOff;

uniform vec2 mouseDownPos; // used for click selection

uniform float focus;
uniform float far;

#define SCENE
#define pi2 6.283185307179586476925286766559

// polynomial smooth min (k = 0.1); by iq
float smin(float a, float b, float k)
{
    float h = clamp(0.5+0.5*(b-a)/k, 0.0, 1.0);
    return mix(b, a, h) - k*h*(1.0-h);
}


vec3 rY(vec3 v, float t) 
{
	float COS = cos(t);
	float SIN = sin(t);
	return vec3(COS*v.x-SIN*v.z, v.y, SIN*v.x+COS*v.z);
}

/*
float sdCuboc(vec3 p, vec3 r) // by vincent francois, https://www.shadertoy.com/view/lssXW7
{
	return
		max(
			length(p) - length(r) -
			(abs(r.x)+abs(r.y)+abs(r.z)) * 0.33,
			max(abs(p.x) - r.x, 0.0) - 0.5 * r.x +
			max(abs(p.y) - r.y, 0.0) - 0.5 * r.y +
			max(abs(p.z) - r.z, 0.0) - 0.5 * r.z
		);			
}

float sdRhombi(vec3 p, vec3 r) {
	return
		max(abs(p.x) - r.x, 0.0) - 0.5 * r.x +
		max(abs(p.y) - r.y, 0.0) - 0.5 * r.y +
		max(abs(p.z) - r.z, 0.0) - 0.5 * r.z;
}
*/

float sdBox(vec3 p, vec3 b)
{	
	vec3 d = abs(p) - b;
	return min(max(d.x,max(d.y,d.z)),0.0) + length(max(d,0.0));
}

float sdSphere(vec3 p, float r)
{
	p.y *= 1.06;
	return length(p)-r;
}

float sdPlane(vec3 p, vec4 n)
{
	n.xyz = normalize(n.xyz);
	return dot(p,n.xyz) + n.w;
}

float sdHexPrism(vec3 p, vec2 h)
{
    vec3 q = abs(p);
    return max(q.z-h.y,max(q.x+q.y*0.57735,q.y*1.1547)-h.x);
}

float sdTriPrism(vec3 p, vec2 h)
{
    vec3 q = abs(p);
    return max(q.z-h.y,max(q.x*0.866025+p.y*0.5,-p.y)-h.x*0.5);
}

float sdTorus(vec3 p, vec2 t)
{
  vec2 q = vec2(length(p.xy)-t.x,p.z);
  return length(q)-t.y;
}

float sdCapsule(vec3 p, vec3 a, vec3 b, float r)
{
    vec3 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

float sdCappedCylinder(vec3 p, vec2 h)
{
  vec2 d = abs(vec2(length(p.xz),p.y)) - h;
  return min(max(d.x,d.y),0.0) + length(max(d,0.0));
}

float opU(float d1, float d2)
{
    return min(d1,d2);
}

float opS(float d1, float d2)
{
    return max(-d1,d2);
}

float opI(float d1, float d2)
{
    return max(d1,d2);
}

vec3 opRep(vec3 p, vec3 c)
{
    return mod(p,c)-0.5*c;
}

vec2 rotate(vec2 p, float a)
{
	vec2 r;
	r.x = p.x*cos(a) - p.y*sin(a);
	r.y = p.x*sin(a) + p.y*cos(a);
	return r;
}

struct Hit
{
	float d;
	vec4 color;
	int id;
};

Hit scene(vec3 p)
{
	float d, d1, d2, d3, d4, d5, f, v;
	int id = 0;
	
	d = 100000.0;
	vec4 col = vec4(1.0);

	// axis
	float r = 0.003;
 	float r2 = r*2.0;
	vec2 map = vec2(1.0, 0.0);

	if (showAxis == 1)
	{
		// red, X-Axis
		d = length(p.yz) - r;
		col = map.xyyy;

		// green, Y-Axis
		d1 = length(p.xz) - r;
		if (d1 < d) { d = d1; col = map.yxyy; }

		// blue, Z-Axis
		d1 = length(p.yx) - r;
		if (d1 < d) { d = d1; col = map.yyxy; }
	}

	if (showGrid == 1 && ((abs(p.x) > r2 && abs(p.z) > r2) || showAxis == 0))
	{
		d1 = length(vec2(p.y, mod(p.x - 0.25, 0.5) - 0.25)) - r;
		if (d1 < d) { d = d1; col = vec4(0.3, 0.3, 0.3, 0.0); }

		d1 = length(vec2(p.y, mod(p.z - 0.25, 0.5) - 0.25)) - r;
		if (d1 < d) { d = d1; col = vec4(0.3, 0.3, 0.3, 0.0); }
	}

	if (showSun == 1)
	{
		d1 = sdSphere(p-lightPosNormalized*2.0, 0.1);
		if (d1 < d)	{ d = d1; col = vec4(1.0, 1.0, 0.0, 1.0); }
	}

	vec3 p0 = p;

	#ifdef SCENE
	#endif
	
	return Hit(d, col, id);
}

vec3 normal(vec3 p)
{
	float c = scene(p).d;
	vec2 h = vec2(0.01, 0.0);
	return normalize(vec3(scene(p + h.xyy).d - c, 
						  scene(p + h.yxy).d - c, 
		                  scene(p + h.yyx).d - c));
}

vec3 colorize(Hit hit, vec3 n, vec3 dir)
{
	float diffuse = diffuseIntensity*max(0.0, dot(n, lightPosNormalized));
	vec3 ref = normalize(reflect(dir, n));
	float specular = specularIntensity*pow(max(0.0, dot(ref, lightPosNormalized)), specularFallOff);
	return (hit.color.rgb + diffuse * diffuseColor + specular * specularColor);
}

void main( void ) 
{
    vec2 pos = (gl_FragCoord.xy*2.0 - resolution.xy) / resolution.y;
	
	bool find = gl_FragCoord.x < 2.0 && gl_FragCoord.y < 2.0; // for click selection
	if (find) pos = (mouseDownPos*2.0 - resolution.xy) / resolution.y;
	
	vec3 cp = camPos;
	vec3 ct = camTarget;
   	vec3 cd = normalize(ct-cp);
    vec3 cu  = vec3(0.0, 1.0, 0.0);
    vec3 cs = cross(cd, cu);
    vec3 dir = normalize(cs*pos.x + cu*pos.y + cd*focus);	
	
	vec3 col = vec3(0.0), ray = cp;
    Hit h = Hit(0.0, vec4(col, 0.0), 0);
	float dist = 0.0;

	for (int i = 0; i < 140; i++)
	{
		h = scene(ray);

		if (h.d < 0.0001) break;

		dist += h.d;
		ray += dir * h.d;

		if (dist > far)
		{
			dist = far;
			break;
		}
	}

	if (find)
	{
		gl_FragColor = vec4(0.04)*float(h.id);
		return;
	}
	
	float m = (1.0 - dist / far);
	if (h.color.a < 0.5) col = h.color.rgb*m; // no shading for axis and grid
	else col = colorize(h, normal(ray), dir)*m;

	gl_FragColor = vec4(pow(col, vec3(1.0/2.2)), 1.0);
}
