
export interface Agency { agencyId: number; name: string; display_name: string; titles: CfrReference[]; }

export interface CfrReference { title: number; }

export interface Snapshot { id: number; retrievedAt: string; wordCount: number; uniqueTermCount: number; changeDensity: number; jaccardSimilarityWithPrevious: number; sha256Checksum: string; }
