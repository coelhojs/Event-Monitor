export interface EventStats {
    counter: number;
    region: string;
    sensor: string;
    tag: string;

    details: EventStats[];
}