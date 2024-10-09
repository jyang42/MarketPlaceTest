export interface BidDTO {
    
    id: number;
    ammount?: number;
    createdDate?: string;
    bidderName?: string;
    jobId?: number
}

export interface JobDTO {
    id: number;
    numberOfBids?: number;
    description?: string;
    requirements?: string;
    highestBid?: number;
    lowestBid?: number;
    createdDate?: string; // Use string for Date representation in JSON
    expirationDate?: string; // Use string for Date representation in JSON
    expired?: boolean;
    bids?: BidDTO[];
    posterId?: string;
    posterName?: string;
}